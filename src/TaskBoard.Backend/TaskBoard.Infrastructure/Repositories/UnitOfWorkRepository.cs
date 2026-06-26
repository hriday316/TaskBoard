using System;
 using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class UnitOfWorkRepository(AppDbContext context) : IUnitOfWorkRepository
{
    private readonly AppDbContext _context = context;

     

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
