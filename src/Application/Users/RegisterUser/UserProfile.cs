using Domain.Entities;

namespace Application.Users.RegisterUser;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<RegisterUserCommand, User>();
    }
}