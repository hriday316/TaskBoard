using System;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces.Repositories;

public interface IWorkspaceRepository
{
    Task AddWorkspaceAsync(Workspace entity);
    Task<List<Workspace>> GetWorkspacesByUserIdAsync(Guid userId);
}
