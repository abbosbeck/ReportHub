namespace Application.Common.Interfaces;

public interface IClientRoleAssignmentRepository
{
    Task<List<string>> GetClientRolesByClientIdAsync(Guid clientId);
}