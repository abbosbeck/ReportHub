using Domain.Entities;

namespace Application.Users.GetUserList;

public class UserProfile : Profile
{
	public UserProfile()
	{
		CreateMap<User, UserDto>();
	}
}
