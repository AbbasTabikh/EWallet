using EWallet.Entities;
using EWallet.Validations.ValidationModels;
using FluentValidation;

namespace EWallet.Validations
{
    public class BudgetValidation : AbstractValidator<BudgetValidationModel>
    {
        public BudgetValidation()
        {
            RuleFor(x => x.Total).Must(x => x > 0)
                                 .WithMessage("Total must be greater than zero");

            RuleFor(x => x.Total).Must(x => x <= Constants.MaxBudget)
                                 .WithMessage("Maximum budget can must be less than 9,999,999,999.99");
        }
    }
}
