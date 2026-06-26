using System;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces.Repositories;

public interface ICardAssignmentRepository
{
    Task  AddAssignment(CardAssignment assignment);
    Task<bool> DeleteAssignmentAsync(Guid assignmentId);
    Task SaveChangesAsync();

}
