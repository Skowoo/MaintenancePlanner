using ActionServiceAPI.Application.Commands.CreateActionCommand;
using ActionServiceAPI.Application.Commands.DeleteActionCommand;
using ActionServiceAPI.Application.Commands.UpdateActionCommand;
using ActionServiceAPI.Application.Queries.GetActionById;
using ActionServiceAPI.Application.Queries.GetAllActions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ActionServiceAPI.Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class ActionController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllActions() 
            => Ok(await mediator.Send(new GetAllActionsQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActionById(int id) 
            => Ok(await mediator.Send(new GetActionByIdQuery(id)));

        [HttpPost]
        public async Task<IActionResult> CreateAction([FromBody] CreateActionCommand command) 
            => Ok(await mediator.Send(command));

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateAction([FromBody] UpdateActionCommand command)
        {
            var result = await mediator.Send(command);
            return result? Ok(command.Id) : NotFound(command.Id);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteAction(int id)
        {
            var result = await mediator.Send(new DeleteActionCommand(id));
            return result ? Ok(id) : NotFound(id);
        }
    }
}
