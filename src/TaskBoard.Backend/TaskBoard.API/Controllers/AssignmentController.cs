using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Assignments.Commands;

namespace TaskBoard.API.Controllers;

[Route("api/cards/{cardId:guid}/assignments")]
[ApiController]
[Authorize]
public class AssignmentController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateAssignment(
        [FromRoute] Guid cardId,
        [FromBody] CreateCardAssigneeCommand command)
    {
        command.CardId = cardId;

        var assignment = await Mediator.Send(command);
        return Ok(assignment);
    }
}
