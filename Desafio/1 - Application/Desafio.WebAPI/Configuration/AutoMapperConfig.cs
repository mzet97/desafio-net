using AutoMapper;
using Desafio.Domain.Models;
using Desafio.WebAPI.ViewModels.Pessoa;

namespace Desafio.WebAPI.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<Pessoa, PessoaCreatedViewModel>();
            CreateMap<Pessoa, PessoaViewModel>();

            CreateMap<PessoaViewModel, Pessoa>();
            CreateMap<PessoaCreatedViewModel, Pessoa>();

        }

    }
}
