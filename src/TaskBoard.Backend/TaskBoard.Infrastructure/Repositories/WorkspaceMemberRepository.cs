using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class WorkspaceMemberRepository(AppDbContext context) : IWorkspaceMemberRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddDefaultWorkspaceMemberAsync(WorkspaceMember workspaceMember)
    {
        await _context.WorkspaceMembers.AddAsync(workspaceMember);
     }

    public async Task<List<WorkspaceMember>> GetWorkspaceMembersAsync(Guid workspaceId)
    {
        return await _context.WorkspaceMembers.Where( x => x.WorkspaceId == workspaceId).Include(x => x.User) .ToListAsync();
    }

    // public async Task<Guid> AddWorkspaceMemberAsync(WorkspaceMember workspaceMember)
    // {
    //     _context.WorkspaceMembers.AddAsync(workspaceMember);
    //     await _context.SaveChangesAsync();
    //     return workspaceMember.Id;
    // }

    public async Task<bool> IsWorkspaceMemberAsync(Guid workspaceId, Guid userId)
    {
        return await _context.WorkspaceMembers.AnyAsync(x => x.WorkspaceId == workspaceId && x.UserId == userId);
    }
    public async Task<WorkspaceMember?> GetWorkspaceMemberByIdAsync(Guid workspaceId , Guid userId)
    {
        return await _context.WorkspaceMembers.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId && x.UserId == userId);
    }
    
    

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
