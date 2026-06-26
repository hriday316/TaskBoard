using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Infrastructure.Repositories;

public class AuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IAuthRepository
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
       return await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<ApplicationUser?> GetUserByIdAsync( Guid id)
    { 
        return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<SignInResult> SignInAsync(string email, string password)
    {
        return await _signInManager.PasswordSignInAsync(email, password, isPersistent:  true, lockoutOnFailure: false);
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
    

    public async Task<bool> UserExists(string email)
    {
        return await _userManager.Users.AnyAsync(u => u.Email == email);
    }
}
