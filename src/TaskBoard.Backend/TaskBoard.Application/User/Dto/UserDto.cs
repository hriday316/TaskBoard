using System;

namespace TaskBoard.Application.User.Dto;

public class UserDto
{
    public  Guid Id { get; set; } 
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; } 


}
