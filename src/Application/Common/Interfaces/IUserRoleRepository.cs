namespace Application.Common.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<string>> GetUserRolesByUserIdAsync(Guid userId);
    }
}
