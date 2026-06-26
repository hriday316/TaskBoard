using System;
namespace TaskBoard.Application.Interfaces.Repositories;

public interface IUnitOfWorkRepository
{
    Task<int> SaveChangesAsync( CancellationToken cancellationToken );
 }
