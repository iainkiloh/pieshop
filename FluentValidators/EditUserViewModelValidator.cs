using FluentValidation;
using Pieshop.ViewModels;
using System.Linq;

namespace Pieshop.FluentValidators
{
    public class EditUserViewModelValidator : AbstractValidator<EditUserViewModel>
    {
        public EditUserViewModelValidator()
        {
            RuleFor(x => x.Roles.Where(p => p.Selected).Count()).GreaterThanOrEqualTo(1).WithMessage("At least one role must be selected");
        }
    }
}
