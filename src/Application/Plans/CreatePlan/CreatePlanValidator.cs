namespace Application.Plans.CreatePlan;
public class CreatePlanValidator : AbstractValidator<CreatePlanCommand>
{
    public CreatePlanValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(p => p.StartDate)
            .LessThan(p => p.EndDate)
            .WithMessage("Start date must be earlier than end date.");
    }
}
