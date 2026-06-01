using MaloteDigital.Endpoints;
using MaloteDigital.InfraStructure.db;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<MaloteDigitalDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Malote Digital API")
            .WithTheme(ScalarTheme.DeepSpace) 
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.AddCondominiumEndpoints();



app.Run();

