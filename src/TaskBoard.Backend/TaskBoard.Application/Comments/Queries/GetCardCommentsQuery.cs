using System;
using AutoMapper;
using MediatR;
using TaskBoard.Application.Comments.Dto;
using TaskBoard.Application.Interfaces.Repositories;

namespace TaskBoard.Application.Comments.Queries;

public class GetCardCommentsQuery : IRequest<List<CommentDto>>
{
    public Guid CardId { get; set; }

    public class Handler(ICommentRepository commentRepository, IMapper mapper) : IRequestHandler<GetCardCommentsQuery, List<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<CommentDto>> Handle(GetCardCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetCommentsByCardIdAsync(request.CardId);
            return _mapper.Map<List<CommentDto>>(comments);
        }
    }
}
