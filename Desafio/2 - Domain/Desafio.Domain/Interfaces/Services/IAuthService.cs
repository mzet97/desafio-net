using Desafio.Domain.Models;
using Desafio.Infra.CrossCutting.ViewModels.User;

namespace Desafio.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseViewModel> GetJwt(string email);
        Task<LoginResponseViewModel> Register(User user, string password);
        Task<LoginResponseViewModel> Login(string email, string password);
    }
}
