using System;
using Microsoft.AspNetCore.Identity;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Infrastructure.Repositories;

public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    public Task<ApplicationUser?> GetCurrentUserAsync( string userId)
    {
        return _userManager.FindByIdAsync(userId);
    }
}
