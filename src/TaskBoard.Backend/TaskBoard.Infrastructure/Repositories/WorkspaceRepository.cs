using System;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Domain.Entities;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class WorkspaceRepository(AppDbContext context) : IWorkspaceRepository
{
    private readonly AppDbContext _context = context;
    public async Task AddWorkspaceAsync(Workspace workspace)
    {
        await _context.Workspaces.AddAsync(workspace);

    }

    public async Task<List<Workspace>> GetWorkspacesByUserIdAsync(Guid userId)
    {
        return await _context.Workspaces.Where(w => w.OwnerId == userId || w.Members.Any(m => m.UserId == userId)).ToListAsync();
    }

}
