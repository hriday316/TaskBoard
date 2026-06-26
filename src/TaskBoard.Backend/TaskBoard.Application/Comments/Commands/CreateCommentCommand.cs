using System;
using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Comments.Dto;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Comments.Commands;

public class CreateCommentCommand : IRequest<CommentDto>
{
    public Guid CardId { get; set; }
    public Guid WorkspaceId { get; set; }
     public string Content { get; set; } = string.Empty;

    public class Handler(IHttpContextAccessor httpContextAccessor,IWorkspaceMemberRepository workspaceMemberRepository, ICommentRepository commentRepository, IMapper mapper) : IRequestHandler<CreateCommentCommand, CommentDto>
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new Exception("User is not authenticated.");
            }
            var workspaceMember = await  _workspaceMemberRepository.GetWorkspaceMemberByIdAsync(request.WorkspaceId, Guid.Parse(userId));
            if (workspaceMember == null)
            {
                throw new Exception("Unauthorized user.");
            }

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                CardId = request.CardId,
                WorkspaceMemberId = workspaceMember.Id,
                UserId = Guid.Parse(userId),
                Content = request.Content,
                CreatedAtUtc = DateTime.UtcNow
            };

             await _commentRepository.CreateCommentAsync(comment);
 
            return  _mapper.Map<CommentDto>(comment);
        }
    }

}
