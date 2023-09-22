using Desafio.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desafio.Infra.Data.Mappings
{
    public class PessoaMapping : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Nome)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.Property(r => r.Sobrenome)
               .HasColumnType("varchar(255)")
               .IsRequired(false);

            builder.Property(r => r.DataNascimento)
               .IsRequired(false);

            builder.Property(r => r.Email)
               .IsRequired()
               .HasColumnType("varchar(255)");

            builder.HasIndex(x => x.Email)
                .HasDatabaseName("UX_Email")
                .IsUnique();

            builder.Property(r => r.Telefone)
                .HasColumnType("varchar(20)")
                .IsRequired(false);

            builder.Property(r => r.Endereco)
                .HasColumnType("varchar(255)")
                .IsRequired(false);

            builder.Property(r => r.Cidade)
                .HasColumnType("varchar(100)")
                .IsRequired(false);

            builder.Property(r => r.Estado)
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            builder.Property(r => r.CEP)
                .HasColumnType("varchar(10)")
                .IsRequired(false);

            builder.Property(r => r.CPFCNPJ)
                .IsRequired()
                .HasColumnType("varchar(14)");

        }
    }
}
