using FluentValidation;
using ProductAPI.Commands;

namespace ProductAPI.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(c => c.ProductId)
                .GreaterThan(0).WithMessage("The ProductId must be greater than zero.");

            RuleFor(x => x.Name)
                .MaximumLength(50).WithMessage("The Name cannot be more than 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));                

            RuleFor(x => x.Status)
                .InclusiveBetween(0, 1).WithMessage("The Status must be either 0 or 1.")
                .When(x => x.Status != null);

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("The Stock cannot be negative.")
                .When(x => x.Stock != null);

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("The Description cannot be more than 200 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("The Price must be greater than zero.")
                .When(x => x.Price != null);
        }
    }
}
