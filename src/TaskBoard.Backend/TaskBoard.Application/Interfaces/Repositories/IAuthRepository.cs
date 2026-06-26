using System;
using Microsoft.AspNetCore.Identity;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<bool> UserExists(string email);
    Task<ApplicationUser?> GetUserByEmailAsync(string email);
    Task<ApplicationUser?> GetUserByIdAsync( Guid id);
    Task<IdentityResult> RegisterAsync(ApplicationUser user, string password);
    Task<SignInResult> SignInAsync(string email, string password);
    Task SignOutAsync();



}
