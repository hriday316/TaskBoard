using System;
using AutoMapper;
using MediatR;
using TaskBoard.Application.Auth.Dto;
using TaskBoard.Application.Boards.Helpers;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Enums;

namespace TaskBoard.Application.Auth.Commands;

public class RegisterUserCommand : IRequest<UserDto>

{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public class Handler(IAuthRepository authRepository,
     IMapper mapper,
     IWorkspaceRepository workspaceRepository,
     IWorkspaceMemberRepository workspaceMemberRepository,
     IBoardRepository boardRepository,
     IBoardColumnRepository boardColumnRepository,
     ILaneRepository laneRepository,
     IUnitOfWorkRepository unitOfWorkRepository) : IRequestHandler<RegisterUserCommand, UserDto>
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IWorkspaceRepository _workspaceRepository = workspaceRepository;
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IBoardRepository _boardRepository = boardRepository;
        private readonly IBoardColumnRepository _boardColumnRepository = boardColumnRepository;
        private readonly ILaneRepository _laneRepository = laneRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository = unitOfWorkRepository;

        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
        
                var isUserExist = await _authRepository.UserExists(request.Email);
                if (isUserExist)
                    throw new Exception("User already exists");
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Email = request.Email,
                    UserName = request.Email, 

                };

                var result = await _authRepository.RegisterAsync(user, request.Password);

                if (!result.Succeeded){ 
                    throw new Exception("Failed to register user");
                }
                var workspace = new Workspace
                {
                    Id = Guid.NewGuid(),
                    Name = "Default Workspace",
                    Description = "Default workspace created upon user registration",
                    OwnerId = user.Id,
                    CreatedAt = DateTime.UtcNow
                };
                var workspaceMember = new WorkspaceMember
                {
                    WorkspaceId = workspace.Id,
                    UserId = user.Id,
                    Role = WorkspaceRole.Owner,
                    JoinedAt = DateTime.UtcNow
                };
                var defaultBoard = BoardDefaultGenerator.GenerateDefaultBoard(workspace.Id);
                var defaultColumns = BoardDefaultGenerator.GenerateDefaultColumns(defaultBoard.Id);
                var defaultLane = BoardDefaultGenerator.GenerateDefaultLane(defaultBoard.Id);


                await _workspaceRepository.AddWorkspaceAsync(workspace);
                await _workspaceMemberRepository.AddDefaultWorkspaceMemberAsync(workspaceMember);
                await _boardRepository.AddBoardAsync(defaultBoard);
                await _boardColumnRepository.AddDefaultColumnsAsync(defaultColumns);
                await _laneRepository.AddLaneAsync(defaultLane);
                await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

                return _mapper.Map<UserDto>(user);
            


        }
    }


}
