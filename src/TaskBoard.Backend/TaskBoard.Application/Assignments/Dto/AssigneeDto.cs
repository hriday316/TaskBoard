using System;

namespace TaskBoard.Application.Assignments.Dto;

public class  AssignmentDto
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public Guid MemberId { get; set; }
    public Guid? UserId { get; set; }
    public string? UserName { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; } = string.Empty;

}
