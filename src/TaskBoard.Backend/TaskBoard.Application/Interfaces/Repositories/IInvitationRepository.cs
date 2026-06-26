using System;
using TaskBoard.Domain.Enums;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces.Repositories;

public interface IInvitationRepository
{
    Task<Guid> SendInvitationAsync(WorkspaceInvitation invitation);
    Task<WorkspaceInvitation?> GetInvitationByIdAsync(Guid id);
    Task<List<WorkspaceInvitation>?> GetInvitationsByEmailAsync(string email);
    void UpdateInvitationStatus(WorkspaceInvitation invitation);
    Task<bool> DeleteInvitationAsync(Guid id);
    Task<List<WorkspaceInvitation>> GetInvitationsByWorkspaceIdAsync(Guid workspaceId);
    Task SaveChangesAsync();

}
