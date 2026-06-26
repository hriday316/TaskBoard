using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Application.Workspaces.Dtos;
using TaskBoard.Application.Boards.Helpers;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Enums;

namespace TaskBoard.Application.Workspaces.Commands;

public class CreateWorkspaceCommand : IRequest<WorkspaceDto>
{
      public string Name { get; set; } = string.Empty;
      public string? Description { get; set; }

      public class Handler(
      IHttpContextAccessor httpContextAccessor,
       IWorkspaceRepository workspaceRepository,
       IWorkspaceMemberRepository workspaceMemberRepository,
       IBoardRepository boardRepository,
       IBoardColumnRepository boardColumnRepository,
       ILaneRepository laneRepository,
       IUnitOfWorkRepository unitOfWorkRepository,
       IMapper mapper)
       : IRequestHandler<CreateWorkspaceCommand, WorkspaceDto>
      {
            private readonly IWorkspaceRepository _workspaceRepository = workspaceRepository;
            private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
            private readonly IBoardRepository _boardRepository = boardRepository;
            private readonly IBoardColumnRepository _boardColumnRepository = boardColumnRepository;
            private readonly ILaneRepository _laneRepository = laneRepository;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository = unitOfWorkRepository;
            private readonly IMapper _mapper = mapper;
            private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
            public async Task<WorkspaceDto> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
            {
                  var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                  if (userId == null){
                        throw new Exception("User not authenticated");
                  }

                  var workspace = new Workspace
                  {
                        Id = Guid.NewGuid(),
                        Name = request.Name,
                        Description = request.Description,
                        OwnerId = Guid.Parse(userId),
                        CreatedAt = DateTime.UtcNow
                  };
                  
                  var workspaceMember = new WorkspaceMember
                  {
                        WorkspaceId = workspace.Id,
                        UserId = Guid.Parse(userId),
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

                  return _mapper.Map<WorkspaceDto>(workspace);
            }
      }

}
