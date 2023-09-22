using System.ComponentModel.DataAnnotations;

namespace Desafio.WebAPI.ViewModels.User
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "O campo {0} é requerido")]
        [EmailAddress(ErrorMessage = "O campo {0} é inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é requeridod")]
        [StringLength(255, ErrorMessage = "O campo  {0} deve está entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
