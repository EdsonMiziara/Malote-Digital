using FluentValidation;
using MaloteDigital.Api.dtos.Create;
using MaloteDigital.Api.dtos.Update;
using MaloteDigital.Domain.Entities;
using MaloteDigital.InfraStructure.db;
using Microsoft.AspNetCore.Mvc;

namespace MaloteDigital.Api.endpoints;

public static class ExpenseExtensions
{
    public static void AddExpenseEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/expenses").WithTags("Expense");
        group.MapPost("/", async ([FromBody] CreateExpenseDto dto,
            [FromServices] IValidator<CreateExpenseDto> validator,
            [FromServices] MaloteDigitalDbContext db) =>
        {

            var validationResults = await validator.ValidateAsync(dto);
            if (!validationResults.IsValid)
            {
                return Results.ValidationProblem(validationResults.ToDictionary());
            }

            var expense = new Expense
            {
                Id = Guid.CreateVersion7(),
                CondominiumId = dto.CondominiumId,
                DetailedDescription = dto.Description ?? string.Empty,
                Amount = dto.Amount,
                DueDate = dto.DueDate
            };

            db.Expenses.Add(expense);
            db.SaveChanges();

            return Results.Created($"/expenses/{expense.Id}", expense);
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

        group.MapGet("/Condominium/{condominiumId}", async (
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
