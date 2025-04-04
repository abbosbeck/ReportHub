using FluentValidation.Validators;

namespace Application.Users.LoginUser
{
    public static class PhoneNumberValidation
    {
        public static IRuleBuilderOptions<T, string> MatchPhoneNumberRule<T>(this IRuleBuilder<T, string> ruleBuilder, string v)
        {
            return ruleBuilder.SetValidator(new RegularExpressionValidator<T>(@"^\+[1-9]\d{9,14}$"));
        }
    }
}
