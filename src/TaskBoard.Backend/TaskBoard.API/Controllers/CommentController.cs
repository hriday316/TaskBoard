using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Comments.Commands;
using TaskBoard.Application.Comments.Queries;

namespace TaskBoard.API.Controllers;

[Route("api/cards/{cardId:guid}/comments")]
[ApiController]
[Authorize]
public class CommentController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateComment(
        
        [FromRoute] Guid cardId,
        [FromBody] CreateCommentCommand command)
    {
        command.CardId = cardId;
    
        var comment = await Mediator.Send(command );
        return Ok(comment);
    }

    [HttpGet]
    public async Task<IActionResult> GetCardComments(
        [FromRoute] Guid cardId,
        CancellationToken cancellationToken)
    {
        var comments = await Mediator.Send(
            new GetCardCommentsQuery { CardId = cardId },
            cancellationToken);

        return Ok(comments);
    }
}
