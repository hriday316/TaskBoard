using System;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces.Repositories;

public interface IWorkspaceMemberRepository
{
    Task AddDefaultWorkspaceMemberAsync(WorkspaceMember workspaceMember);
     Task<bool> IsWorkspaceMemberAsync(Guid workspaceId, Guid userId);
     Task<List<WorkspaceMember>> GetWorkspaceMembersAsync(Guid workspaceId);
    Task<WorkspaceMember?> GetWorkspaceMemberByIdAsync(Guid workspaceId , Guid userId);
    Task SaveChangesAsync();
}
