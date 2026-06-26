using Microsoft.AspNetCore.Identity;

namespace TaskBoard.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<WorkspaceMember> WorkspaceMemberships { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];        
    }
}
