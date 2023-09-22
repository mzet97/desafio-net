using Desafio.Domain.Models;
using Desafio.Infra.CrossCutting.Filters;

namespace Desafio.Domain.Interfaces.Services
{
    public interface IPessoaService
    {
        Task<Pessoa> Create(Pessoa categoryProduct);
        Task<List<Pessoa>> GetAll();
        Task<List<Pessoa>> GetAllWithFilter(PessoaFilter categoryProductFilter);
        Task<Pessoa> GetById(int id);
        Task<Pessoa> Update(Pessoa categoryProduct);
        Task<Pessoa> Delete(int id);
    }
}
