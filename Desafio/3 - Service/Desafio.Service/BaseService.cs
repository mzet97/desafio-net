using Desafio.Domain.Models;
using Desafio.Infra.CrossCutting.Notifications;
using FluentValidation;
using FluentValidation.Results;

namespace Desafio.Service
{
    public abstract class BaseService
    {
        private readonly INotifier _notifier;

        protected BaseService(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected void NotifyError(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                NotifyError(error.ErrorMessage);
            }
        }

        protected void NotifyError(string mensagem)
        {
            _notifier.Handle(new Notification(mensagem));
        }

        protected bool Validate<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validation.Validate(entity);

            if (validator.IsValid) return true;

            NotifyError(validator);

            return false;
        }
    }
}
