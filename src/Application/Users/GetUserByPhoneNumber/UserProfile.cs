using Domain.Entities;

namespace Application.Users.GetUserByPhoneNumber;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}