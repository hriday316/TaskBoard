using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Boards.Queries;
using TaskBoard.Application.Boards.Commands;

namespace TaskBoard.API.Controllers;

[Route("api/workspaces/{workspaceId:guid}/boards")]
[ApiController]
[Authorize]
public class BoardController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateBoard(Guid workspaceId, [FromBody] CreateBoardCommand command)
    {
        command.WorkspaceId = workspaceId;
        return Ok(await Mediator.Send(command));
    }

    [HttpGet]
    public async Task<IActionResult> GetBoards(Guid workspaceId)
    {
        return Ok(await Mediator.Send(new GetBoardsQuery { WorkspaceId = workspaceId }));
    }

    [HttpGet("{boardId:guid}")]
    public async Task<IActionResult> GetBoard(Guid workspaceId, Guid boardId)
    { return Ok(await Mediator.Send(new GetBoardViewQuery
        {
            WorkspaceId = workspaceId,
            BoardId = boardId
        }));
    }

    [HttpPut("{boardId:guid}")]
    public async Task<IActionResult> UpdateBoard(Guid workspaceId, Guid boardId, [FromBody] UpdateBoardCommand command)
    {
        command.WorkspaceId = workspaceId;
        command.BoardId = boardId;
        return Ok(await Mediator.Send(command));
    }
}
