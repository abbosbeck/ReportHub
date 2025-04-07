using Domain.Entities;

namespace Application.Users.RegisterUser;

public class RegisterUserProfile : Profile
{
    public RegisterUserProfile()
    {
        CreateMap<User, RegisterUserDto>();
    }
}
