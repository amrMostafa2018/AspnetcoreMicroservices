using FluentValidation;


namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandlerValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandHandlerValidator()
        {
            RuleFor(n => n.UserName)
                .NotEmpty()
                .WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

            RuleFor(n => n.EmailAddress)
                .NotEmpty()
                .WithMessage("{EmailAddress} is required.");

            RuleFor(n => n.TotalPrice)
              .NotEmpty()
              .WithMessage("{TotalPrice} is required.")
              .GreaterThan(0).WithMessage("{TotalPrice} should be greater than Zero.");
        }
    }
}
