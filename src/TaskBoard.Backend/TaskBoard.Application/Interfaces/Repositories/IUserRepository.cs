using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> GetCurrentUserAsync( string userId);

}
