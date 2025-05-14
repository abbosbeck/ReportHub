namespace Application.Plans.CreatePlan;
public class CreatePlanRequestValidator : AbstractValidator<CreatePlanRequest>
{
    public CreatePlanRequestValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(p => p.StartDate)
            .LessThan(p => p.EndDate)
            .WithMessage("Start date must be earlier than end date.");

        /*RuleFor(p => p.PlanItems)
            .Must(planItems => planItems.All(p => p.Quantity > 0))
            .WithMessage("Quantity must be greater than 0");*/
    }
}
