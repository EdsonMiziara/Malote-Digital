using FluentValidation;
using MaloteDigital.Domain.interfaces;
using MaloteDigital.Domain.Interfaces;
using MaloteDigital.Endpoints;
using MaloteDigital.InfraStructure.db;
using MaloteDigital.InfraStructure.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<MaloteDigitalDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<ITextParserService, TextParserService>();
builder.Services.AddScoped<IPdfReaderService, PdfReaderService>();
builder.Services.AddScoped<IStorageService, LocalStorageService>();
builder.Services.AddScoped<IOfxParserService, OfxParserService>();
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<IAuditService, ConsoleAuditService>();

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

