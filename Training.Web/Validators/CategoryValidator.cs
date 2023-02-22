using FluentValidation;
using Training.Web.Models;

namespace Training.Web.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(category => category.Name)
                .NotEmpty().WithMessage("Назва категорії не може бути порожньою")
                .Length(3, 50).WithMessage("Назва категорії повинна бути від 3 до 50 символів");
            RuleFor(category => category.Commision)
                .NotEmpty().WithMessage("Комісія не може бути порожньою");
        }
    }
}
