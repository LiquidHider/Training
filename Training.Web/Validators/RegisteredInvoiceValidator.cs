using FluentValidation;
using Training.Web.Models;

namespace Training.Web.Validators
{
    public class RegisteredInvoiceValidator : AbstractValidator<RegisteredInvoice>
    {
        public RegisteredInvoiceValidator()
        {
            RuleFor(registeredInvoice => registeredInvoice.ReceiptDate)
                .GreaterThanOrEqualTo(registeredInvoice => registeredInvoice.StorageDate).WithMessage("Дата прийняття не може бути меншою за Дату закінчення зберігання")
                .LessThanOrEqualTo(DateTime.Now.AddMinutes(-1)).WithMessage("Дата прийняття не може бути меншою за поточну дату");
            RuleFor(registeredInvoice => registeredInvoice.Good)
                .NotNull().WithMessage("Товар є обов'язковим");
            RuleFor(registeredInvoice => registeredInvoice.Client)
                .NotNull().WithMessage("Клієнт є обов'язковим");
        }
    }
}
