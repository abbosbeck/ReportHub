namespace Application.Common.ExtensionMethods;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, Guid> BeAValidGuid<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder.Must(g => Guid.TryParse(g.ToString(), out _))
            .WithMessage("The Guid should be valid!");
    }
}
