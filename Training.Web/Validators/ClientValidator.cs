using FluentValidation;
using Training.Web.Models;

namespace Training.Web.Validators
{
    public class ClientValidator : AbstractValidator<Client>
    {
        public ClientValidator()
        {
            RuleFor(client => client.FullName)
                .NotEmpty().WithMessage("П.І.Б. клієнта не може бути порожнім")
                .Length(10, 50).WithMessage("П.І.Б. клієнта має бути від 10 до 50 символів");
            RuleFor(client => client.PassportSerial)
                .MaximumLength(8).WithMessage("Серія Паспорту клієнта не може перевищувати 8 символів");
            RuleFor(client => client.PassportNumber)
                .NotEmpty().WithMessage("Номер Паспорту клієнта не може бути порожнім")
                .Length(6, 9).WithMessage("Номер Паспорту клієнта має бути від 6 до 9 символів");
            RuleFor(client => client.PhoneNumber)
                .NotEmpty().WithMessage("Номер Телефону клієнта не може бути порожнім")
                .Matches(@"^\+380\d{9}$").WithMessage("Номер Телефону клієнта невірного формату\n+380*********");
        }
    }
}
