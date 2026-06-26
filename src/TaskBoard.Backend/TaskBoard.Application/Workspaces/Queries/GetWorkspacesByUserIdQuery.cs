using System;
using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Application.Workspaces.Dtos;

namespace TaskBoard.Application.Workspaces.Queries;

public class GetWorkspacesByUserIdQuery : IRequest<List<WorkspaceDto>>
{
    public class Handler(IHttpContextAccessor httpContextAccessor, IWorkspaceRepository workspaceRepository, IMapper mapper) : IRequestHandler<GetWorkspacesByUserIdQuery, List<WorkspaceDto>>
    {
        private readonly IWorkspaceRepository _workspaceRepository = workspaceRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<List<WorkspaceDto>> Handle(GetWorkspacesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if ( userId == null)
                throw new Exception("User not authenticated");

            var result = await _workspaceRepository.GetWorkspacesByUserIdAsync(Guid.Parse(userId));
            if (result == null){
                throw new Exception("No workspaces found for the user");
            }

            return _mapper.Map<List<WorkspaceDto>>(result);
        }
    }

}
