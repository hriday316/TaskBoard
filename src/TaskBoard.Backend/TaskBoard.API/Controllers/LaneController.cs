using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Lanes.Commands;

namespace TaskBoard.API.Controllers;

[Route("api/workspaces/{workspaceId:guid}/boards/{boardId:guid}/lanes")]
[ApiController]
[Authorize]
public class LaneController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateLane(Guid workspaceId, Guid boardId, [FromBody] CreateLaneCommand command)
    {
        command.WorkspaceId = workspaceId;
        command.BoardId = boardId;
        return Ok(await Mediator.Send(command));
    }

    [HttpPut("{laneId:guid}")]
    public async Task<IActionResult> UpdateLane(Guid workspaceId, Guid boardId, Guid laneId, [FromBody] UpdateLaneCommand command)
    {
        command.WorkspaceId = workspaceId;
        command.BoardId = boardId;
        command.LaneId = laneId;
        return Ok(await Mediator.Send(command));
    }
}
