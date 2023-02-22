using FluentValidation;
using Training.Web.Models;

namespace Training.Web.Validators
{
    public class GoodValidator : AbstractValidator<Good>
    {
        public GoodValidator()
        {
            RuleFor(good => good.Name)
                .NotEmpty().WithMessage("Назва товару не може бути порожньою")
                .Length(3, 50).WithMessage("Назва товару повинна бути від 3 до 50 символів");
            RuleFor(good => good.Description)
                .MaximumLength(100).WithMessage("Опис товару не може бути більше 100 символів");
            RuleFor(good => good.AppraisedValue)
                .NotEmpty().WithMessage("Оціночна вартість не може бути попрожньою");
            RuleFor(good => good.Category)
                .NotNull().WithMessage("Категорія є обов'язковою");
        }
    }
}
