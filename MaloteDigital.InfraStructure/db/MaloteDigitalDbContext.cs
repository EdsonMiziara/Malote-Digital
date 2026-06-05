using MaloteDigital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaloteDigital.InfraStructure.db;

public class MaloteDigitalDbContext : DbContext
{
    public MaloteDigitalDbContext(DbContextOptions<MaloteDigitalDbContext> options) : base(options)
    {
    }
    public DbSet<Condominium> Condominiums { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Condominium>(entity =>
        {
            entity.ToTable("Condominiums");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.PreferredPaymentDate).IsRequired();
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.ToTable("Expenses");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Beneficiary).IsRequired().HasMaxLength(150);
            entity.Property(b => b.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(b => b.Status).IsRequired().HasMaxLength(30);
            entity.Property(b => b.Observation).HasMaxLength(500);

            entity.HasOne(b => b.Condominium)
                  .WithMany(c => c.Expenses)
                  .HasForeignKey(b => b.CondominiumId)
                  .OnDelete(DeleteBehavior.Cascade); 
        });

        modelBuilder.Entity<Expense>()
        .HasIndex(e => new { e.CondominiumId, e.Amount, e.DueDate })
        .IsUnique()
        .HasDatabaseName("IX_Expense_Unique_Condo_Amount_DueDate");
    }
}
