using ActionServiceAPI.Application.Commands.CreateActionCommand;
using ActionServiceAPI.Application.Queries.GetAllActions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ActionServiceAPI.Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class ActionController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllActions([FromServices] IMediator mediator)
        {
            var actions = await mediator.Send(new GetAllActionsQuery());
            return Ok(actions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAction([FromServices] IMediator mediator, [FromBody] CreateActionCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }
    }
}
