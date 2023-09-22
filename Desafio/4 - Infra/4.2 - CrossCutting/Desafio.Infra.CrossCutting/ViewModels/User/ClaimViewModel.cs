using System.Security.Claims;

namespace Desafio.Infra.CrossCutting.ViewModels.User
{
    public class ClaimViewModel
    {
        public string Value { get; set; }
        public string Type { get; set; }

        public ClaimViewModel()
        {

        }

        public ClaimViewModel(Claim claim)
        {
            Value = claim.Value;
            Type = claim.Type;
        }
    }
}
