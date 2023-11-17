using EWallet.Validations.ValidationModels;
using FluentValidation;

namespace EWallet.Validations
{
    public class ExpenseValidation : AbstractValidator<ExpenseValidationModel>
    {
        public ExpenseValidation()
        {
            RuleFor(x => x.Price).Must(x => x >= 0).When(y => y.Price != null)
                                                   .WithMessage("Price must be greater than zero");
            RuleFor(x => x.Name).NotEmpty()
                                .NotNull();

            RuleFor(x => x.Name).Must(x => x.Length <= 22)
                                .When(x => x.Name != null)
                                .WithMessage("Name maximum 22 characters");
        }
    }
}
    