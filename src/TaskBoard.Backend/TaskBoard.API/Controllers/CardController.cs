using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Cards.Commands;
using TaskBoard.Application.Cards.Dto;
using TaskBoard.Application.Cards.Model;
using TaskBoard.Application.Cards.Queries;
using TaskBoard.Application.Interfaces.Services;

namespace TaskBoard.API.Controllers
{
    [Route("api/workspaces/{workspaceId:guid}/boards/{boardId:guid}/cards")]
    [ApiController]
    [Authorize]
    public class CardController(IMediator mediator, IFileService fileService) : BaseController(mediator)
    {
        private readonly IFileService _fileService = fileService;

        [HttpPost]
        public async Task<IActionResult> CreateCard(
            [FromRoute] Guid workspaceId,
            [FromRoute] Guid boardId,
            [FromForm] CreateCardRequest request)
        {
            var attachments = new List<string>();


            if (request.Attachments != null)
            {
                var filePath = await _fileService.UploadFilesAsync(request.Attachments);
                attachments = filePath ?? [];
            }

            var command = new CreateCardCommand
            {
                Title = request.Title,
                Description = request.Description,
                AssignMemberId = request.AssignMemberId,
                BoardColumnId = request.BoardColumnId,
                LaneId = request.LaneId,
                 Attachments = attachments,
                WorkspaceId = workspaceId,
                BoardId = boardId
            };

            var card = await Mediator.Send(command);

            return Ok(card);
        }

        [HttpPut("{cardId:guid}/move")]
        public async Task<IActionResult> MoveCard(Guid workspaceId, Guid boardId, Guid cardId, [FromBody] MoveCardCommand command)
        {
            command.WorkspaceId = workspaceId;
            command.BoardId = boardId;
            command.CardId = cardId;
            await Mediator.Send(command);
            return Ok();
        }

        [HttpGet("{cardId:guid}")]
        public async Task<IActionResult> GetCardById(
            [FromRoute] Guid cardId)
        {
            var card = await Mediator.Send(new GetCardByIdQuery { Id = cardId });

            return Ok(card);
        }
    }
}
