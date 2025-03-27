namespace Application.Common.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string userName)
            : base($"User not found with name {userName}")
        {
        }
    }
}
