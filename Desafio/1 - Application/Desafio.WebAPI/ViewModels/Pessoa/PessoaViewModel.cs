using System.ComponentModel.DataAnnotations;

namespace Desafio.WebAPI.ViewModels.Pessoa
{
    public class PessoaViewModel : PessoaCreatedViewModel
    {
        [Key]
        public int Id { get; set; }
    }
}
