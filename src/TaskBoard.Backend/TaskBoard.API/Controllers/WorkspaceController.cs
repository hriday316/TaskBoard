using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.WorkSpaceMembers.Queries;
using TaskBoard.Application.Workspaces.Commands;
using TaskBoard.Application.Workspaces.Queries;

namespace TaskBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkspaceController(IMediator mediator) : BaseController(mediator)
    {
         [HttpPost]
        public async Task<IActionResult> CreateWorkspace([FromBody] CreateWorkspaceCommand command)
        {
            var workspace = await Mediator.Send(command);
            return Ok(workspace);
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkspacesByUserId()
        {
            var  result = await Mediator.Send(new GetWorkspacesByUserIdQuery());
            Console.WriteLine("Workspaces loaded successfully:", result);
            return Ok(result);
        }

        [HttpGet("{workspaceId:guid}/members")]
        public async Task<IActionResult> GetWorkspaceMembers([FromRoute] Guid workspaceId)
        {
            var result = await Mediator.Send(new GetWorkspaceMembersQuery { WorkspaceId = workspaceId });
            return Ok(result);
        }

    }
}
