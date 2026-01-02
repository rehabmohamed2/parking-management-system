using FluentValidation;
using IntegrationServices.DTOs;

namespace IntegrationServices.DtoValidators
{
    public class CreateInvoiceRequestDtoValidator : AbstractValidator<CreateInvoiceRequestDto>
    {
        public CreateInvoiceRequestDtoValidator()
        {
            RuleFor(x => x.InvoiceHTMLDocument)
                .NotEmpty();

            RuleFor(x => x.TicketSerial)
                .NotEmpty();

            RuleFor(x => x.PlateNumber)
                .NotEmpty();

            RuleFor(x => x.InvoiceId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(x => x.TicketId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(x => x.FromDate)
                .NotEmpty()
                .Must(BeValidDate)
                .WithMessage("FromDate must be a valid date");

            RuleFor(x => x.ToDate)
                .NotEmpty()
                .Must(BeValidDate)
                .WithMessage("ToDate must be a valid date");


            RuleFor(x => x)
                .Must(x => x.ToDate > x.FromDate)
                .WithMessage("ToDate must be after FromDate");

            RuleFor(x => x.TotalAmountWithOutTax)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.TaxAmount)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.PlateNumber)
                .Matches(SaudiPlateRegex)
                .WithMessage("PlateNumber must be a valid Saudi plate number");

            RuleFor(x => x.TicketSerial)
                .Matches(@"^\d{9}$")
                .WithMessage("TicketSerial must be exactly 9 digits");
        }

        private static bool BeValidDate(DateTime date)
        {
            return date != default;
        }
        // Saudi plate format examples:
        // ABC 1234
        // AB 1234
        // A 1234
        private const string SaudiPlateRegex =
            @"^\d{1,4}[A-Z]{3}$";
    }
}
