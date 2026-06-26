using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.BoardColumns.Commands;

namespace TaskBoard.API.Controllers
{
    [Route("api/workspaces/{workspaceId}/boards/{boardId}")]
    [ApiController]
    [Authorize]
    public class BoardColumnController(IMediator mediator) : BaseController(mediator)
    {
        [HttpPost("columns")]
        public async Task<IActionResult> CreateBoardColumn(Guid workspaceId, Guid boardId, [FromBody] CreateBoardColumnCommand command)
        {
            command.WorkspaceId = workspaceId;
            command.BoardId = boardId;
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("columns/{columnId:guid}")]
        public async Task<IActionResult> UpdateBoardColumn(Guid workspaceId, Guid boardId, Guid columnId, [FromBody] UpdateBoardColumnCommand command)
        {
            command.WorkspaceId = workspaceId;
            command.BoardId = boardId;
            command.ColumnId = columnId;
            return Ok(await Mediator.Send(command));
        }
    }
}
