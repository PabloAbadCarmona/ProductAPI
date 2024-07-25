using FluentValidation;
using ProductAPI.Commands;

namespace ProductAPI.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(c => c.ProductId)
                .GreaterThan(0).WithMessage("The ProductId must be greater than zero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The Name cannot be empty.")
                .MaximumLength(50).WithMessage("The Name cannot be more than 50 characters.");

            RuleFor(x => x.Status)
                .InclusiveBetween(0, 1).WithMessage("The Status must be either 0 or 1.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("The Stock cannot be negative.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("The Description cannot be empty.")
                .MaximumLength(200).WithMessage("The Description cannot be more than 200 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("The Price must be greater than zero.");
        }
    }
}
