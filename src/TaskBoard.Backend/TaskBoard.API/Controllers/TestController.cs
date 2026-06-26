using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Tests;
using TaskBoard.Application.Tests.Commands;
using TaskBoard.Application.Tests.Queries;

namespace TaskBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : BaseController
    {
        public TestController(IMediator mediator) : base(mediator) { }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await Mediator.Send(new GetAllTestsQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TestDto testDto)
        {
            var result = await Mediator.Send(new CreateTestCommand { Test = testDto });
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTestCommand command)
        {
            command.Id = id;
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     var result = await Mediator.Send(new DeleteTestCommand(id));
        //     return Ok(result);
        // }
    }
}
