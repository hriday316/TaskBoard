using System;

namespace TaskBoard.Application.Comments.Dto;

public class CommentDto
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public Guid WorkspaceMemberId { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }

}
