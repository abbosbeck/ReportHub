using Domain.Entities;

namespace Application.Users.GetUserByEmail;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}