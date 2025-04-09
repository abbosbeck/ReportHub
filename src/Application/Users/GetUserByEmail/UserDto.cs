namespace Application.Users.GetUserByEmail;

public class UserDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Department { get; set; }

    public string PhoneNumber { get; set; }
}