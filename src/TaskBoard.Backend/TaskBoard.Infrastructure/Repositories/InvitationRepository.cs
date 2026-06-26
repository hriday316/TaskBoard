using System;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Domain.Enums;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class InvitationRepository(AppDbContext context) : IInvitationRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Guid> SendInvitationAsync(WorkspaceInvitation invitation)
    {
        _context.WorkspaceInvitations.Add(invitation);
        await _context.SaveChangesAsync();
        return invitation.Id;
    }
 
    public async Task<bool> DeleteInvitationAsync(Guid id)
    {
        var invitation = await _context.WorkspaceInvitations.FirstOrDefaultAsync(x => x.Id == id);
        if (invitation == null) return false;

        _context.WorkspaceInvitations.Remove(invitation);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<WorkspaceInvitation?> GetInvitationByIdAsync(Guid id)
    {
        return await _context.WorkspaceInvitations.Include(x => x.Workspace).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<WorkspaceInvitation>?> GetInvitationsByEmailAsync(string email)
    {
        return await _context.WorkspaceInvitations.Include(x => x.Workspace).Where(x => x.InviteeEmail == email).ToListAsync();
    }

    public async Task<List<WorkspaceInvitation>> GetInvitationsByWorkspaceIdAsync(Guid workspaceId)
    {
        return await _context.WorkspaceInvitations.Where(x => x.WorkspaceId == workspaceId).Include(x => x.Workspace).ToListAsync();
    }

    public void UpdateInvitationStatus(WorkspaceInvitation invitation)
    {
        _context.WorkspaceInvitations.Update(invitation);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
