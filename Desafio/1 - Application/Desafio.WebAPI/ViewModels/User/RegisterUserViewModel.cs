using System.ComponentModel.DataAnnotations;

namespace Desafio.WebAPI.ViewModels.User
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "O campo {0} é requerido")]
        [EmailAddress(ErrorMessage = "O campo {0} é inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é requeridod")]
        [StringLength(255, ErrorMessage = "O campo  {0} deve está entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "A Senha não bate")]
        public string ConfirmPassword { get; set; }


        public Domain.Models.User ToDomain()
        {
            var entity = new Domain.Models.User();

            entity.UserName = Email;
            entity.Email = Email;
            entity.EmailConfirmed = true;

            return entity;
        }
    }
}
