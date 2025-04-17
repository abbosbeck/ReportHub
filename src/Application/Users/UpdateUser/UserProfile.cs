using Domain.Entities;

namespace Application.Users.UpdateUser;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UpdateUserCommand, User>();
        CreateMap<User, UserDto>();
    }
}
