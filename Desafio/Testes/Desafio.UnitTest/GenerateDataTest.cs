using Bogus;
using Bogus.Extensions.Brazil;
using Desafio.Domain.Models;

namespace Desafio.UnitTest
{
    public class GenerateDataTest
    {
        public Pessoa GetValidCPF()
        {
            var pessoaGenerator = new Faker<Pessoa>("pt_BR")
                .RuleFor(u => u.Nome, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.Sobrenome, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email())
                .RuleFor(u => u.DataNascimento, (f, u) => f.Date.Between(new DateTime(1900, 1, 1), new DateTime(2022, 1, 1)))
                .RuleFor(u => u.Telefone, (f, u) => f.Phone.PhoneNumber())
                .RuleFor(u => u.Endereco, (f, u) => f.Address.FullAddress())
                .RuleFor(u => u.Cidade, (f, u) => f.Address.City())
                .RuleFor(u => u.Estado, (f, u) => f.Address.State())
                .RuleFor(u => u.CEP, (f, u) => f.Address.ZipCode())
                .RuleFor(u => u.CPFCNPJ, (f, u) => f.Person.Cpf(false));

            return pessoaGenerator.Generate();
        }

        public Pessoa GetValidCNPJ()
        {
            var pessoaGenerator = new Faker<Pessoa>("pt_BR")
                .RuleFor(u => u.Nome, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.Sobrenome, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email())
                .RuleFor(u => u.DataNascimento, (f, u) => f.Date.Between(new DateTime(1900, 1, 1), new DateTime(2022, 1, 1)))
                .RuleFor(u => u.Telefone, (f, u) => f.Phone.PhoneNumber())
                .RuleFor(u => u.Endereco, (f, u) => f.Address.FullAddress())
                .RuleFor(u => u.Cidade, (f, u) => f.Address.City())
                .RuleFor(u => u.Estado, (f, u) => f.Address.State())
                .RuleFor(u => u.CEP, (f, u) => f.Address.ZipCode())
                .RuleFor(u => u.CPFCNPJ, (f, u) => f.Company.Cnpj(false));

            return pessoaGenerator.Generate();
        }

        public Pessoa GetInvalid()
        {
            return new Pessoa(0, "1", "1", new DateTime(), "email", "1", "1", "1", "1", "1", "1");
        }
    }
}
