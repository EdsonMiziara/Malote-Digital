using FluentValidation;
using MaloteDigital.Api.dtos.Create;
using MaloteDigital.Api.dtos.Update;
using MaloteDigital.Domain;
using MaloteDigital.Domain.Entities;
using MaloteDigital.Domain.interfaces;
using MaloteDigital.Domain.Interfaces;
using MaloteDigital.Domain.ValueObjects;
using MaloteDigital.InfraStructure.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaloteDigital.Api.endpoints;

public static class ExpenseExtensions
{
    public static void AddExpenseEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/expenses").WithTags("Expense");


        group.MapPost("/", async ([FromBody] CreateExpenseDto dto,
            [FromServices] IValidator<CreateExpenseDto> validator,
            [FromServices] MaloteDigitalDbContext db) =>
        {

            var validationResults = await validator.ValidateAsync(dto);

            if (!validationResults.IsValid) return Results.ValidationProblem(validationResults.ToDictionary());

            var condominium = await db.Condominiums.FindAsync(dto.CondominiumId);

            if (condominium is null) return Results.NotFound("Condomínio não encontrado.");

            var expense = new Expense
            {
                Id = Guid.CreateVersion7(),
                CondominiumId = dto.CondominiumId,
                DetailedDescription = dto.Description ?? string.Empty,
                Amount = dto.Amount,
                DueDate = dto.DueDate
            };

            expense.CalculatePreferredDate(condominium.PreferredPaymentDate);

            db.Expenses.Add(expense);
            db.SaveChanges();

            return Results.Created($"/api/expenses/{expense.Id}", expense);
        });

        group.MapPost("/upload", async (
    IFormFileCollection files,
    Guid condominiumId,
    [FromServices] IHashService hashService,
    [FromServices] ITextParserService textParserService,
    [FromServices] IStorageService storageService,
    [FromServices] IPdfReaderService pdfReaderService,
    [FromServices] MaloteDigitalDbContext db) =>
        {
            if (files is null || files.Count == 0)
                return Results.BadRequest("Nenhum arquivo enviado.");

            var condominium = await db.Condominiums.FindAsync(condominiumId);
            if (condominium is null)
                return Results.NotFound("Condomínio não encontrado no sistema.");

            foreach (var file in files)
            {
                if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                    continue;

                using var stream = file.OpenReadStream();

                string fileHash = hashService.ComputeHash(stream);

                stream.Position = 0;

                bool isDuplicate = await db.Expenses.AnyAsync(e => e.FileHash == fileHash);
                if (isDuplicate)
                    return Results.Conflict($"O arquivo '{file.FileName}' já foi importado anteriormente.");

                string relativePath = await storageService.UploadFileAsync(stream, file.FileName, "pdf");

                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.TrimStart('/'));

                string rawText = await pdfReaderService.ExtractTextAsync(fullPath);
                var extractedData = textParserService.ParseExpenseData(rawText);

                var expense = new Expense
                {
                    Id = Guid.CreateVersion7(),
                    PdfUrl = relativePath,
                    FileHash = fileHash,
                    CondominiumId = condominiumId,
                    DetailedDescription = $"Importado via arquivo: {file.FileName}",
                    Amount = extractedData.Amount,
                    DueDate = extractedData.DueDate ?? DateTime.UtcNow.AddDays(7),
                    IssueDate = extractedData.IssueDate
                };

                expense.CalculatePreferredDate(condominium.PreferredPaymentDate);

                db.Expenses.Add(expense);
            }

            await db.SaveChangesAsync();

            return Results.Ok(new { message = $"{files.Count} arquivos processados com sucesso." });
        });

        group.MapPost("/conciliation", async (
            IFormFile file,
            Guid condominiumId,
            [FromServices] IOfxParserService ofxParserService,
            [FromServices] IAuditService auditService,
            [FromServices] MaloteDigitalDbContext db) =>
        {
            if (file is null || file.Length == 0)
                return Results.BadRequest("Nenhum arquivo enviado.");
            if (Path.GetExtension(file.FileName).ToLower() != ".ofx")
                return Results.BadRequest("Arquivo inválido. Por favor, envie um arquivo OFX.");

            using var stream = file.OpenReadStream();
            List<OfxTransactionResult> bankDebits = ofxParserService.ParseDebits(stream);

            if (!bankDebits.Any())
                return Results.Ok(new { message = "Nenhuma transação de débito encontrada no arquivo OFX." });

            var pendingExpenses = await db.Expenses
                .Where(e => e.CondominiumId == condominiumId && e.Status == "Pendente")
                .ToListAsync();

            int reconciledCount = 0;
            var logs = new List<string>();
            try
            {
                foreach (var debit in bankDebits)
                {
                    var matchedExpense = pendingExpenses.FirstOrDefault(e =>
                        e.Amount == debit.Amount &&
                    debit.DatePosted >= e.DueDate.AddDays(-3) &&
                    debit.DatePosted <= e.DueDate.AddDays(3));

                    if (matchedExpense != null)
                    {
                        matchedExpense.FinalizePayment(debit.DatePosted);

                        reconciledCount++;
                        logs.Add($"Boleto '{matchedExpense.DetailedDescription}' conciliado com débito bancário de {debit.Amount} em {debit.DatePosted:d}.");

                        pendingExpenses.Remove(matchedExpense);
                    }
                }
                if (reconciledCount > 0)
                {
                    await db.SaveChangesAsync();

                    var auditDetails = $"Conciliação automatizada via arquivo OFX '{file.FileName}'. Total de {reconciledCount} despesas baixadas de forma atômica.";

                    await auditService.LogAsync(new AuditLogResult(
                        Action: "EXECUTE_BANK_RECONCILIATION",
                        Details: auditDetails
                    ));
                }
            }
            catch (BusinessException ex)
            {
                logs.Add($"Erro durante a conciliação: {ex.Message}");
                return Results.BadRequest(new { error = ex.Message });
            }

            return Results.Ok(new
            {
                message = "Processamento do extrato OFX concluído.",
                reconciledTransactions = reconciledCount,
                details = logs
            });

        });

        group.MapGet("/", async (
            [FromServices] MaloteDigitalDbContext db) =>
        {
            var expenses = db.Expenses.ToList();

            return Results.Ok(expenses);
        });

        group.MapGet("/{id}", async (
            Guid id,
            [FromServices] MaloteDigitalDbContext db) =>
        {
            var expense = await db.Expenses.FindAsync(id);
            if (expense is null)
            {
                return Results.NotFound(new { Message = "Boleto não encontrado." });
            }
            return Results.Ok(expense);
        });

        group.MapGet("/ByCondominium/{condominiumId}", async (
            Guid condominiumId,
            [FromServices] MaloteDigitalDbContext db) =>
        {
            var expenses = db.Expenses
                .Where(e => e.CondominiumId == condominiumId)
                .ToList();
            return Results.Ok(expenses);
        });

        group.MapPut("/{id}/status", async (
            Guid id,
            [FromBody] UpdateExpenseStatusDto dto,
            [FromServices] IValidator<UpdateExpenseStatusDto> validator,
            [FromServices] MaloteDigitalDbContext db) =>
        {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var expense = await db.Expenses.FindAsync(id);
            if (expense is null)
            {
                return Results.NotFound(new { Message = "Boleto não encontrado." });
            }

            expense.Status = dto.Status;
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }

}
