using System;
using TaskBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Application.Interfaces.Services;
using TaskBoard.Infrastructure.Repositories;
using TaskBoard.Infrastructure.Services;


namespace TaskBoard.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(
                    connectionString,
                    npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null
                    )
                );
            });

            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();
            services.AddScoped<IWorkspaceMemberRepository, WorkspaceMemberRepository>();
            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddScoped<IBoardColumnRepository, BoardColumnRepository>();
            services.AddScoped<ILaneRepository, LaneRepository>();
            services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<ICardAssignmentRepository, CardAssignmentRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}
