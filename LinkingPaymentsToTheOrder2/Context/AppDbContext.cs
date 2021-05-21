using System;
using System.Collections.ObjectModel;
using System.Configuration;
using LinkingPaymentsToTheOrder2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace LinkingPaymentsToTheOrder2.Context
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MoneyComing> MoneyComings { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }

        public void LoadData(AppDbContext context)
        {
            context.Orders.Load();
            context.MoneyComings.Load();
            context.Payments.Load();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<MoneyComing>(entity =>
            {
                entity.HasKey(e => e.Number)
                    .HasName("PK__MoneyCom__78A1A19CE83F89D3");

                entity.Property(e => e.Balance).HasColumnType("money");

                entity.Property(e => e.DatePayment).HasColumnType("datetime");

                entity.Property(e => e.Summ).HasColumnType("money");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Number)
                    .HasName("PK__Orders__78A1A19C8FA14CA3");

                entity.Property(e => e.DateOrder).HasColumnType("datetime");

                entity.Property(e => e.Summ).HasColumnType("money");

                entity.Property(e => e.SummPay)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.SummPay).HasColumnType("money");

                entity.HasOne(d => d.NumberMoneyComingNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.NumberMoneyComing)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payement_To_MoneyComings");

                entity.HasOne(d => d.NumberOrderNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.NumberOrder)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payement_To_Orders");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
