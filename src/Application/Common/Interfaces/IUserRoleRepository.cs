namespace Application.Common.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<string> GetUserRolesByUserIdAsync(Guid userId);
    }
}
