namespace Application.Plans.AddPlanItem;

public class AddPlanItemCommandValidator : AbstractValidator<AddPlanItemRequest>
{
    public AddPlanItemCommandValidator()
    {
        RuleFor(planItem => planItem.Quantity)
            .GreaterThan(0);
    }
}