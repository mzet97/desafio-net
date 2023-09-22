using Desafio.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Desafio.Infra.Data.Context
{
    public class DataIdentityDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DataIdentityDbContext(DbContextOptions<DataIdentityDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataIdentityDbContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<ZonedDateTime>(x => x.HaveConversion<ZonedDateTimeConverter>());
        }
    }

    internal class ZonedDateTimeConverter : ValueConverter<ZonedDateTime, LocalDateTime>
    {
        public ZonedDateTimeConverter() :
           base(v => v.WithZone(DateTimeZone.Utc).LocalDateTime, v => v.InUtc())
        {
        }
    }
}
