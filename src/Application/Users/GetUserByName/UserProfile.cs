using Domain.Entity;

namespace Application.Users.GetUserByName;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}