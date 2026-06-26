using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Invitations.Commands;
using TaskBoard.Application.Invitations.Queries;

namespace TaskBoard.API.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class InvitationController(IMediator mediator) : BaseController(mediator)
    {
        [HttpPost("workspaces/{workspaceId:guid}/invitations")]
        public async Task<IActionResult> SendInvitation([FromRoute] Guid WorkspaceId, [FromBody] SendInvitationCommand command)
        {
            command.WorkspaceId = WorkspaceId;
            var invitationId = await Mediator.Send(command);
            return Ok(invitationId);
        }

        [HttpGet("invitations/{InvitationId:guid}")]
        public async Task<IActionResult> GetInvitationDetails([FromRoute] Guid InvitationId)
        {
            var result = await Mediator.Send(new GetInviationDteailsQuery { InvitationId = InvitationId });
            return Ok(result);
        }

        [HttpPut("invitations/{InvitationId:guid}")]
        public async Task<IActionResult> UpdateInvitationStatus([FromRoute] Guid InvitationId, [FromBody] UpdateInvitationStatusCommand command)
        {
            command.InvitationId = InvitationId;
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("invitations")]
        public async Task<IActionResult> GetMyInvitations()
        {
            var result = await Mediator.Send(new GetMyInvitationQuery());
            return Ok(result);
        }
    }
}
