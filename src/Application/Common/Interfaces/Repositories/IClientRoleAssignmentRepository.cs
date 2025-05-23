﻿using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IClientRoleAssignmentRepository
{
    Task AddAsync(ClientRoleAssignment clientRoleAssignment);

    Task<List<string>> GetByUserIdAsync(Guid userId);

    Task<List<string>> GetRolesByUserIdAndClientIdAsync(Guid userId, Guid clientId);

    IQueryable<ClientRoleAssignment> GetAll();
}
