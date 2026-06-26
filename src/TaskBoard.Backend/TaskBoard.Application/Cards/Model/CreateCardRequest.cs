using Microsoft.AspNetCore.Http;

namespace TaskBoard.Application.Cards.Model;

public class CreateCardRequest{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? AssignMemberId { get; set; }
     public Guid BoardColumnId { get; set; }
    public Guid LaneId { get; set; }
    public IEnumerable<IFormFile> Attachments { get; set; } = new List<IFormFile>();

}