using FluentValidation;
using MaloteDigital.Domain.Entities;
using MaloteDigital.InfraStructure.db;
using Microsoft.EntityFrameworkCore;

namespace MaloteDigital.Endpoints;

public static class CondominiumExtensions
{
    public static void AddCondominiumEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/condominiums").WithTags("Condominium");

        group.MapPost("/", async (
            CreateCondominiumDto dto,
            IValidator<CreateCondominiumDto> validator,
            MaloteDigitalDbContext db) =>
        {
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var condominio = new Condominium
            {
                Id = Guid.CreateVersion7(),
                Name = dto.Name,
                PreferredPaymentDate = dto.PreferredPaymentDate
            };

            db.Condominiums.Add(condominio);
            await db.SaveChangesAsync();

            return Results.Created($"/condominiums/{condominio.Id}", condominio);
        });

        group.MapGet("/", async (MaloteDigitalDbContext db) =>
        {
            var Condominiums = await db.Condominiums
                .Select(c => new { c.Id, c.Name, c.PreferredPaymentDate })
                .ToListAsync();

            return Results.Ok(Condominiums);
        });
    }
}

public record CreateCondominiumDto(string Name, int PreferredPaymentDate);