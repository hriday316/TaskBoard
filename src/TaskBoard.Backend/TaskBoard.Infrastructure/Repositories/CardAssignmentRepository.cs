using System;
using TaskBoard.Application.Assignments.Dto;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class CardAssignmentRepository(AppDbContext context) : ICardAssignmentRepository
{
    private readonly AppDbContext _context = context;
    public async Task  AddAssignment(CardAssignment assignment)
    {
         await _context.CardAssignments.AddAsync(assignment);
         
        
    }

    public async Task<bool> DeleteAssignmentAsync(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
