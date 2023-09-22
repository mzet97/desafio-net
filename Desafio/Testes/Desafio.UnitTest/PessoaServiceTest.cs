using Desafio.Infra.CrossCutting.Filters;
using Desafio.Infra.CrossCutting.Notifications;
using Desafio.Infra.Data.Context;
using Desafio.Infra.Data.Repository;
using Desafio.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Desafio.UnitTest
{
    public class PessoaServiceTest : BaseTest, IClassFixture<BaseTest>
    {
        private ServiceProvider _serviceProvide;

        public PessoaServiceTest(BaseTest baseTest)
        {
            _serviceProvide = baseTest.ServiceProvider;
        }

        [Fact(DisplayName = "Test Create success CPF")]
        [Trait("PessoaServiceTest", "PessoaServiceTest Unit Tests")]
        public async Task TestCreateWithSucessCPF()
        {
            using (var context = _serviceProvide.GetService<DataIdentityDbContext>())
            {
                var notifier = new NotifierNoLog();
                var pessoaRepository = new PessoaRepository(context);

                var pessoaService = new PessoaService(notifier, pessoaRepository);

                var pessoa = new GenerateDataTest().GetValidCPF();

                await pessoaService.Create(pessoa);

                var pessoaDb = await pessoaService.GetAllWithFilter(new PessoaFilter()
                {
                    CPFCNPJ = pessoa.CPFCNPJ
                });

                Assert.NotNull(context);
                Assert.NotNull(notifier);
                Assert.NotNull(pessoaRepository);
                Assert.NotNull(pessoaService);
                Assert.NotNull(pessoa);
                Assert.True(!notifier.HasNotification());
                Assert.True(pessoaDb.Count > 0);
                Assert.Contains(pessoaDb, x => x.Nome == pessoa.Nome);
            }
        }

        [Fact(DisplayName = "Test Create success CNPJ")]
        [Trait("PessoaServiceTest", "PessoaServiceTest Unit Tests")]
        public async Task TestCreateWithSucessCNPJ()
        {
            using (var context = _serviceProvide.GetService<DataIdentityDbContext>())
            {
                var notifier = new NotifierNoLog();
                var pessoaRepository = new PessoaRepository(context);

                var pessoaService = new PessoaService(notifier, pessoaRepository);

                var pessoa = new GenerateDataTest().GetValidCNPJ();

                pessoa.CPFCNPJ = "46920257000162";
                await pessoaService.Create(pessoa);

                var pessoaDb = await pessoaService.GetAllWithFilter(new PessoaFilter()
                {
                    CPFCNPJ = pessoa.CPFCNPJ
                });

                Assert.NotNull(context);
                Assert.NotNull(notifier);
                Assert.NotNull(pessoaRepository);
                Assert.NotNull(pessoaService);
                Assert.NotNull(pessoa);
                Assert.True(pessoaDb.Count > 0);
                Assert.True(!notifier.HasNotification());
                Assert.Contains(pessoaDb, x => x.Nome == pessoa.Nome);
            }
        }


        [Fact(DisplayName = "Test Create Failure")]
        [Trait("PessoaServiceTest", "PessoaServiceTest Unit Tests")]
        public async Task TestCreateWithFailure()
        {
            using (var context = _serviceProvide.GetService<DataIdentityDbContext>())
            {
                var notifier = new NotifierNoLog();
                var pessoaRepository = new PessoaRepository(context);

                var pessoaService = new PessoaService(notifier, pessoaRepository);

                var pessoa = new GenerateDataTest().GetInvalid();

                await pessoaService.Create(pessoa);

                Assert.NotNull(context);
                Assert.NotNull(notifier);
                Assert.NotNull(pessoaRepository);
                Assert.NotNull(pessoaService);
                Assert.NotNull(pessoa);
                Assert.True(notifier.HasNotification());
            }
        }

        [Fact(DisplayName = "Test Get Failure")]
        [Trait("PessoaServiceTest", "PessoaServiceTest Unit Tests")]
        public async Task TestGetFailure()
        {
            using (var context = _serviceProvide.GetService<DataIdentityDbContext>())
            {
                var notifier = new NotifierNoLog();
                var pessoaRepository = new PessoaRepository(context);

                var pessoaService = new PessoaService(notifier, pessoaRepository);

                var pessoaDb = await pessoaService.GetById(-1);

                Assert.NotNull(context);
                Assert.NotNull(notifier);
                Assert.NotNull(pessoaRepository);
                Assert.NotNull(pessoaService);
                Assert.Null(pessoaDb);
            }
        }

        [Fact(DisplayName = "Test Get success")]
        [Trait("PessoaServiceTest", "PessoaServiceTest Unit Tests")]
        public async Task TestGet()
        {
            using (var context = _serviceProvide.GetService<DataIdentityDbContext>())
            {
                var notifier = new NotifierNoLog();
                var pessoaRepository = new PessoaRepository(context);

                var pessoaService = new PessoaService(notifier, pessoaRepository);

                var pessoaDb = await pessoaService.GetAllWithFilter(new PessoaFilter());

                Assert.NotNull(context);
                Assert.NotNull(notifier);
                Assert.NotNull(pessoaRepository);
                Assert.NotNull(pessoaService);
                Assert.True(!notifier.HasNotification());
                Assert.True(pessoaDb.Count > 0);
            }
        }

        [Fact(DisplayName = "Test Update success")]
        [Trait("PessoaServiceTest", "PessoaServiceTest Unit Tests")]
        public async Task TestUpdateWithSucessCPF()
        {
            using (var context = _serviceProvide.GetService<DataIdentityDbContext>())
            {
                var bogusData = new Bogus.DataSets.Lorem(locale: "pt_BR");
                var notifier = new NotifierNoLog();
                var pessoaRepository = new PessoaRepository(context);

                var pessoaService = new PessoaService(notifier, pessoaRepository);

                var pessoa = new GenerateDataTest().GetValidCPF();

                await pessoaService.Create(pessoa);

                var pessoaDb = await pessoaService.GetAllWithFilter(new PessoaFilter()
                {
                    CPFCNPJ = pessoa.CPFCNPJ
                });

                var pessoaUp = pessoaDb.First();

                pessoaUp.Nome = bogusData.Word();

                await pessoaService.Update(pessoaUp);

                var pessoaUpDb = await pessoaService.GetById(pessoaUp.Id);

                Assert.NotNull(context);
                Assert.NotNull(notifier);
                Assert.NotNull(pessoaRepository);
                Assert.NotNull(pessoaService);
                Assert.NotNull(pessoa);
                Assert.True(!notifier.HasNotification());
                Assert.True(pessoaDb.Count > 0);
                Assert.Equal(pessoaUp.Id, pessoaUpDb.Id);
            }
        }

        [Fact(DisplayName = "Test Update failure")]
        [Trait("PessoaServiceTest", "PessoaServiceTest Unit Tests")]
        public async Task TestUpdateWithFailureCPF()
        {
            using (var context = _serviceProvide.GetService<DataIdentityDbContext>())
            {
                var bogusData = new Bogus.DataSets.Lorem(locale: "pt_BR");
                var notifier = new NotifierNoLog();
                var pessoaRepository = new PessoaRepository(context);

                var pessoaService = new PessoaService(notifier, pessoaRepository);

                var pessoa = new GenerateDataTest().GetValidCPF();

                await pessoaService.Create(pessoa);

                var pessoaDb = await pessoaService.GetAllWithFilter(new PessoaFilter()
                {
                    CPFCNPJ = pessoa.CPFCNPJ
                });

                var pessoaUp = pessoaDb.First();

                pessoaUp.Nome = bogusData.Word();

                pessoaUp.Id = -1;

                await pessoaService.Update(pessoaUp);

                var pessoaUpDb = await pessoaService.GetById(pessoaUp.Id);

                Assert.NotNull(context);
                Assert.NotNull(notifier);
                Assert.NotNull(pessoaRepository);
                Assert.NotNull(pessoaService);
                Assert.NotNull(pessoa);
                Assert.True(notifier.HasNotification());
                Assert.True(pessoaDb.Count > 0);
                Assert.Null(pessoaUpDb);
            }
        }

        [Fact(DisplayName = "Test Delete success")]
        [Trait("PessoaServiceTest", "PessoaServiceTest Unit Tests")]
        public async Task TestDeleteWithSucess()
        {
            using (var context = _serviceProvide.GetService<DataIdentityDbContext>())
            {
                var notifier = new NotifierNoLog();
                var pessoaRepository = new PessoaRepository(context);

                var pessoaService = new PessoaService(notifier, pessoaRepository);

                var pessoa = new GenerateDataTest().GetValidCPF();

                await pessoaService.Create(pessoa);

                var pessoaDb = await pessoaService.GetAllWithFilter(new PessoaFilter()
                {
                    CPFCNPJ = pessoa.CPFCNPJ
                });

                await pessoaService.Delete(pessoaDb.First().Id);

                var pessoaDelete = await pessoaService.GetAllWithFilter(new PessoaFilter()
                {
                    CPFCNPJ = pessoa.CPFCNPJ
                });

                Assert.NotNull(context);
                Assert.NotNull(notifier);
                Assert.NotNull(pessoaRepository);
                Assert.NotNull(pessoaService);
                Assert.NotNull(pessoa);
                Assert.True(!notifier.HasNotification());
                Assert.Contains(pessoaDb, x => x.Nome == pessoa.Nome);
                Assert.Empty(pessoaDelete);
            }
        }

        [Fact(DisplayName = "Test Delete failure")]
        [Trait("PessoaServiceTest", "PessoaServiceTest Unit Tests")]
        public async Task TestDeleteWithFailure()
        {
            using (var context = _serviceProvide.GetService<DataIdentityDbContext>())
            {
                var notifier = new NotifierNoLog();
                var pessoaRepository = new PessoaRepository(context);

                var pessoaService = new PessoaService(notifier, pessoaRepository);

                await pessoaService.Delete(-1);

                Assert.NotNull(context);
                Assert.NotNull(notifier);
                Assert.NotNull(pessoaRepository);
                Assert.NotNull(pessoaService);
                Assert.True(notifier.HasNotification());
            }
        }
    }
}
