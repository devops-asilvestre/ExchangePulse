using Microsoft.EntityFrameworkCore;
using ExchangePulse.Domain.Entities;

namespace ExchangePulse.Infrastructure.Persistence
{
    public class ExchangePulseDbContext : DbContext
    {
        public ExchangePulseDbContext(DbContextOptions<ExchangePulseDbContext> options)
            : base(options)
        {
        }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<ExchangeMetric> ExchangeMetrics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Currency
            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id)
                      .HasDefaultValueSql("NEWID()")   // GUID gerado no banco
                      .ValueGeneratedOnAdd();

                entity.Property(c => c.Code)
                      .HasMaxLength(3)
                      .IsRequired();
                entity.HasIndex(c => c.Code).IsUnique();

                entity.Property(c => c.Name)
                      .HasMaxLength(100)
                      .IsRequired();
                entity.Property(c => c.Country)
                      .HasMaxLength(100)
                      .IsRequired();
            });

            // ExchangeRate
            modelBuilder.Entity<ExchangeRate>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id)
                      .HasDefaultValueSql("NEWID()")
                      .ValueGeneratedOnAdd();

                entity.Property(r => r.Date).IsRequired();
                entity.HasOne(r => r.Currency)
                      .WithMany()
                      .HasForeignKey(r => r.CurrencyId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(r => r.BuyPrice).HasColumnType("decimal(18,6)");
                entity.Property(r => r.SellPrice).HasColumnType("decimal(18,6)");

                // Ignora propriedades calculadas
                entity.Ignore(r => r.Spread);
                entity.Ignore(r => r.Average);
            });

            // ExchangeMetric
            modelBuilder.Entity<ExchangeMetric>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id)
                      .HasDefaultValueSql("NEWID()")
                      .ValueGeneratedOnAdd();

                entity.Property(m => m.Date).IsRequired();
                entity.HasOne(m => m.Currency)
                      .WithMany()
                      .HasForeignKey(m => m.CurrencyId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(m => m.DailyVariation).HasColumnType("decimal(18,6)");
                entity.Property(m => m.LogReturn).HasColumnType("decimal(18,6)");
                entity.Property(m => m.MovingAverage7d).HasColumnType("decimal(18,6)");
                entity.Property(m => m.MovingAverage30d).HasColumnType("decimal(18,6)");
                entity.Property(m => m.Volatility30d).HasColumnType("decimal(18,6)");
                entity.Property(m => m.SharpeDaily).HasColumnType("decimal(18,6)");
                entity.Property(m => m.SharpeAnnual).HasColumnType("decimal(18,6)");
                entity.Property(m => m.Drawdown).HasColumnType("decimal(18,6)");
                entity.Property(m => m.Beta).HasColumnType("decimal(18,6)");
                entity.Property(m => m.VaREmpirical95).HasColumnType("decimal(18,6)");
                entity.Property(m => m.VaRCornishFisher95).HasColumnType("decimal(18,6)");
                entity.Property(m => m.InterestRate).HasColumnType("decimal(18,6)");
                entity.Property(m => m.Inflation).HasColumnType("decimal(18,6)");
            });
        }
    }
}
