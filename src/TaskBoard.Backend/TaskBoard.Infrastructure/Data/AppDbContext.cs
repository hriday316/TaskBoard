using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Domain.Entities;


namespace TaskBoard.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
    {

        public Guid currentUserId { get; set; } = Guid.Empty;

        public DbSet<Test> Tests { get; set; } = null!;
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public DbSet<Workspace> Workspaces { get; set; } = null!;
        public DbSet<WorkspaceMember> WorkspaceMembers { get; set; } = null!;
        public DbSet<WorkspaceInvitation> WorkspaceInvitations { get; set; } = null!;
        public DbSet<Board> Boards { get; set; } = null!;
        public DbSet<BoardColumn> BoardColumns { get; set; } = null!;
        public DbSet<Lane> Lanes { get; set; } = null!;
        public DbSet<Card> Cards { get; set; } = null!;
        public DbSet<CardAssignment> CardAssignments { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ConfigureApplicationUser(builder);
            ConfigureWorkspace(builder);
            ConfigureWorkspaceMember(builder);
            ConfigureWorkspaceInvitation(builder);
            ConfigureBoard(builder);
            ConfigureBoardColumn(builder);
            ConfigureLane(builder);
            ConfigureCard(builder);
            ConfigureCardAssignment(builder);
            ConfigureComment(builder);
        }

        private static void ConfigureApplicationUser(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(e =>
            {
                e.ToTable("Users");
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.HasIndex(x => x.Email).IsUnique();

            });
        }

        private static void ConfigureWorkspace(ModelBuilder builder)
        {
            builder.Entity<Workspace>(e =>
            {

                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(150);

                e.HasOne(x => x.Owner)
                    .WithMany()
                    .HasForeignKey(x => x.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(x => x.Members)
                    .WithOne(x => x.Workspace)
                    .HasForeignKey(x => x.WorkspaceId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasMany(x => x.Invitations)
                    .WithOne(x => x.Workspace)
                    .HasForeignKey(x => x.WorkspaceId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(x => x.Boards)
                    .WithOne(x => x.Workspace)
                    .HasForeignKey(x => x.WorkspaceId)
                    .OnDelete(DeleteBehavior.Cascade);

            });
        }

        private static void ConfigureWorkspaceMember(ModelBuilder builder)
        {
            builder.Entity<WorkspaceMember>(e =>
            {

                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.WorkspaceId, x.UserId }).IsUnique();
                e.HasOne(x => x.User)
                    .WithMany(x => x.WorkspaceMemberships)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Workspace)
                    .WithMany(x => x.Members)
                    .HasForeignKey(x => x.WorkspaceId)
                    .OnDelete(DeleteBehavior.Cascade);
                

            });
        }

        private static void ConfigureWorkspaceInvitation(ModelBuilder builder)
        {
            builder.Entity<WorkspaceInvitation>(e =>
            {
                e.Property(x => x.InviteeEmail).IsRequired();
                e.HasKey(x => x.Id);

                e.HasOne(x => x.Workspace)
                    .WithMany(x => x.Invitations)
                    .HasForeignKey(x => x.WorkspaceId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.InviteBy)
                    .WithMany()
                    .HasForeignKey(x => x.InviteById)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
        private static void ConfigureBoard(ModelBuilder builder)
        {
            builder.Entity<Board>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Workspace)
                    .WithMany(x => x.Boards)
                    .HasForeignKey(x => x.WorkspaceId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(x => x.Columns)
                    .WithOne(x => x.Board)
                    .HasForeignKey(x => x.BoardId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(x => x.Lanes)
                    .WithOne(x => x.Board)
                    .HasForeignKey(x => x.BoardId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }

        private static void ConfigureBoardColumn(ModelBuilder builder)
        {
            builder.Entity<BoardColumn>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Board)
                    .WithMany(x => x.Columns)
                    .HasForeignKey(x => x.BoardId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(x => x.Cards)
                    .WithOne(x => x.BoardColumn)
                    .HasForeignKey(x => x.BoardColumnId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureLane(ModelBuilder builder)
        {
            builder.Entity<Lane>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.HasOne(x => x.Board)
                    .WithMany(x => x.Lanes)
                    .HasForeignKey(x => x.BoardId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(x => x.Cards)
                    .WithOne(x => x.Lane)
                    .HasForeignKey(x => x.LaneId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureCard(ModelBuilder builder)
        {
            builder.Entity<Card>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasOne(x => x.BoardColumn)
                    .WithMany(x => x.Cards)
                    .HasForeignKey(x => x.BoardColumnId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Lane)
                    .WithMany(x => x.Cards)
                    .HasForeignKey(x => x.LaneId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(x => new { x.BoardColumnId, x.LaneId, x.Order });

                e.HasMany(x => x.Assignments)
                    .WithOne(x => x.Card)
                    .HasForeignKey(x => x.CardId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        private static void ConfigureCardAssignment(ModelBuilder builder)
        {
            builder.Entity<CardAssignment>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.CardId, x.WorkspaceMemberId }).IsUnique();
                e.HasOne(x => x.Card)
                    .WithMany(x => x.Assignments)
                    .HasForeignKey(x => x.CardId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.WorkspaceMember)
                    .WithMany(x => x.CardAssignments)
                    .HasForeignKey(x => x.WorkspaceMemberId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
        private static void ConfigureComment(ModelBuilder builder)
        {
            builder.Entity<Comment>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne<Card>()
                    .WithMany(c => c.Comments)
                    .HasForeignKey(c => c.CardId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(c => c.WorkspaceMember)
                    .WithMany(m => m.Comments)
                    .HasForeignKey(c => c.WorkspaceMemberId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
