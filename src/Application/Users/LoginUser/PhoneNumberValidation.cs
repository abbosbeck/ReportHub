using FluentValidation.Validators;

namespace Application.Users.LoginUser
{
    public static class PhoneNumberValidation
    {
        public static IRuleBuilderOptions<T, string> MatchPhoneNumberRule<T>(this IRuleBuilder<T, string> ruleBuilder, string v)
        {
            return ruleBuilder.SetValidator(new RegularExpressionValidator<T>(@"((?:[0-9]\-?){6,14}[0-9]$)|((?:[0-9]\x20?){6,14}[0-9]$)"));
        }
    }
}
