namespace Application.Users.LoginUser
{
    public class GetUserByNumberValidator : AbstractValidator<LoginUserCommandReqest>
    {
        public GetUserByNumberValidator()
        {
            RuleFor(u => u.PhoneNumber).NotEmpty().MatchPhoneNumberRule("Please provide valid phone number");
        }
    }
}
