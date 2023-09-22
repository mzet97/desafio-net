using Desafio.Domain.Interfaces.Repository;
using Desafio.Domain.Interfaces.Services;
using Desafio.Domain.Models;
using Desafio.Domain.Models.Validations;
using Desafio.Infra.CrossCutting.Filters;
using Desafio.Infra.CrossCutting.Notifications;
using LinqKit;
using System.Linq.Expressions;

namespace Desafio.Service
{
    public class PessoaService : BaseService, IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaService(
            INotifier notifier, 
            IPessoaRepository pessoaRepository) : base(notifier)
        {
            _pessoaRepository = pessoaRepository;
        }

        public async Task<Pessoa> Create(Pessoa pessoa)
        {
            if(!Validate(new PessoaValidation(), pessoa)) return null;

            var pessoaBD = await _pessoaRepository
                .Search(x => x.Email == pessoa.Email);

            if (pessoaBD != null && pessoaBD.Any())
            {
                NotifyError("Já existe uma pessoa com mesmo e-mail");
                return await Task.FromResult<Pessoa>(null);
            }

            await _pessoaRepository
                .Add(pessoa);

            if (await _pessoaRepository.Commit())
            {
                return await Task.FromResult(pessoa);
            }

            NotifyError("Erro ao inserir");
            return await Task.FromResult<Pessoa>(null);
        }

       
        public async Task<List<Pessoa>> GetAll()
        {
            return await _pessoaRepository
                .GetAll();
        }

        public async Task<List<Pessoa>> GetAllWithFilter(PessoaFilter PessoaFilter)
        {
            Expression<Func<Pessoa, bool>> filter = null;
            Func<IQueryable<Pessoa>, IOrderedQueryable<Pessoa>> ordeBy = null;

            #region Campos
            if (!string.IsNullOrWhiteSpace(PessoaFilter.Nome))
            {
                if (filter == null)
                {
                    filter = PredicateBuilder.New<Pessoa>(true);
                }

                filter = filter.And(x => x.Nome == PessoaFilter.Nome || x.Nome.Contains(PessoaFilter.Nome));
            }

            if (!string.IsNullOrWhiteSpace(PessoaFilter.Sobrenome))
            {
                if (filter == null)
                {
                    filter = PredicateBuilder.New<Pessoa>(true);
                }

                filter = filter.And(x => x.Sobrenome == PessoaFilter.Sobrenome || x.Sobrenome.Contains(PessoaFilter.Sobrenome));
            }

            if (!string.IsNullOrWhiteSpace(PessoaFilter.Email))
            {
                if (filter == null)
                {
                    filter = PredicateBuilder.New<Pessoa>(true);
                }

                filter = filter.And(x => x.Email == PessoaFilter.Email || x.Email.Contains(PessoaFilter.Email));
            }

            if (!string.IsNullOrWhiteSpace(PessoaFilter.Telefone))
            {
                if (filter == null)
                {
                    filter = PredicateBuilder.New<Pessoa>(true);
                }

                filter = filter.And(x => x.Telefone == PessoaFilter.Telefone || x.Telefone.Contains(PessoaFilter.Telefone));
            }

            if (!string.IsNullOrWhiteSpace(PessoaFilter.Endereco))
            {
                if (filter == null)
                {
                    filter = PredicateBuilder.New<Pessoa>(true);
                }

                filter = filter.And(x => x.Endereco == PessoaFilter.Endereco || x.Endereco.Contains(PessoaFilter.Endereco));
            }

            if (!string.IsNullOrWhiteSpace(PessoaFilter.Cidade))
            {
                if (filter == null)
                {
                    filter = PredicateBuilder.New<Pessoa>(true);
                }

                filter = filter.And(x => x.Cidade == PessoaFilter.Cidade || x.Cidade.Contains(PessoaFilter.Cidade));
            }

            if (!string.IsNullOrWhiteSpace(PessoaFilter.Estado))
            {
                if (filter == null)
                {
                    filter = PredicateBuilder.New<Pessoa>(true);
                }

                filter = filter.And(x => x.Estado == PessoaFilter.Estado || x.Estado.Contains(PessoaFilter.Estado));
            }

            if (!string.IsNullOrWhiteSpace(PessoaFilter.CEP))
            {
                if (filter == null)
                {
                    filter = PredicateBuilder.New<Pessoa>(true);
                }

                filter = filter.And(x => x.CEP == PessoaFilter.CEP || x.CEP.Contains(PessoaFilter.CEP));
            }

            if (!string.IsNullOrWhiteSpace(PessoaFilter.CPFCNPJ))
            {
                if (filter == null)
                {
                    filter = PredicateBuilder.New<Pessoa>(true);
                }

                filter = filter.And(x => x.CPFCNPJ == PessoaFilter.CPFCNPJ || x.CPFCNPJ.Contains(PessoaFilter.CPFCNPJ));
            }

            #endregion

            if (!string.IsNullOrWhiteSpace(PessoaFilter.Order))
            {
                switch (PessoaFilter.Order)
                {
                    case "Id":
                        ordeBy = x => x.OrderBy(n => n.Id);
                        break;
                    case "Nome":
                        ordeBy = x => x.OrderBy(n => n.Nome);
                        break;
                    case "Sobrenome":
                        ordeBy = x => x.OrderBy(n => n.Sobrenome);
                        break;
                    case "DataNascimento":
                        ordeBy = x => x.OrderBy(n => n.DataNascimento);
                        break;
                    case "Email":
                        ordeBy = x => x.OrderBy(n => n.Email);
                        break;
                    case "Telefone":
                        ordeBy = x => x.OrderBy(n => n.Email);
                        break;
                    case "Endereco":
                        ordeBy = x => x.OrderBy(n => n.Email);
                        break;
                    case "Cidade":
                        ordeBy = x => x.OrderBy(n => n.Email);
                        break;
                    case "Estado":
                        ordeBy = x => x.OrderBy(n => n.Email);
                        break;
                    case "CEP":
                        ordeBy = x => x.OrderBy(n => n.Email);
                        break;
                    case "CPFCNPJ":
                        ordeBy = x => x.OrderBy(n => n.Email);
                        break;
                }
            }

            if (PessoaFilter.Id > 0)
            {
                if (filter == null)
                {
                    filter = PredicateBuilder.New<Pessoa>(true);
                }

                filter = filter.And(x => x.Id == PessoaFilter.Id);
            }

            if (PessoaFilter.DataNascimento.HasValue &&
                PessoaFilter.DataNascimento.Value.Year > 1)
            {
                if (filter == null)
                {
                    filter = PredicateBuilder.New<Pessoa>(true);
                }

                filter = filter.And(x => x.Id == PessoaFilter.Id);
            }

            return await _pessoaRepository
                .Search(
                    filter,
                    ordeBy,
                    PessoaFilter.PageSize,
                    PessoaFilter.PageIndex);
        }

        public async Task<Pessoa> GetById(int id)
        {
            return await _pessoaRepository
                .GetById(id);
        }

        public async Task<Pessoa> Update(Pessoa pessoa)
        {
            if (!Validate(new PessoaValidation(), pessoa)) return null;

            var pessoaDb = await _pessoaRepository
                .GetById(pessoa.Id);

            if (pessoaDb == null)
            {
                NotifyError("Not found");
                return await Task.FromResult<Pessoa>(null);
            }

            pessoa.Email = pessoaDb.Email;
            pessoaDb = pessoa;

            _pessoaRepository
                 .Update(pessoaDb);

            if (await _pessoaRepository.Commit())
            {
                return await Task.FromResult<Pessoa>(pessoaDb);
            }

            NotifyError("Erro ao atualizar");
            return await Task.FromResult<Pessoa>(null);
        }

        public async Task<Pessoa> Delete(int id)
        {
            var Pessoa = await _pessoaRepository
                 .GetById(id);

            if (Pessoa == null)
            {
                NotifyError("Não encontrado");
                return await Task.FromResult<Pessoa>(null);
            }

            _pessoaRepository
                  .Remove(id);

            if (await _pessoaRepository.Commit())
            {
                return await Task.FromResult(Pessoa);
            }

            NotifyError("Erro ao deletar");
            return await Task.FromResult<Pessoa>(null);
        }
    }
}
