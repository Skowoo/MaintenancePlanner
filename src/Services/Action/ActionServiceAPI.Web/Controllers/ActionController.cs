using ActionServiceAPI.Application.Action.Commands.CreateActionCommand;
using ActionServiceAPI.Application.Action.Commands.DeleteActionCommand;
using ActionServiceAPI.Application.Action.Commands.UpdateActionCommand;
using ActionServiceAPI.Application.Action.Queries.GetActionById;
using ActionServiceAPI.Application.Action.Queries.GetAllActions;
using ActionServiceAPI.Application.DataTransferObjects.Models;
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
        [ProducesResponseType(typeof(IEnumerable<ActionDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllActions()
            => Ok(await mediator.Send(new GetAllActionsQuery()));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ActionDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAction(int id)
        {
            var result = await mediator.Send(new GetActionByIdQuery(id));
            return result is not null ? Ok(result) : NotFound(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAction([FromBody] CreateActionCommand command)
        {
            var result = await mediator.Send(command);
            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpPut]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAction([FromBody] UpdateActionCommand command)
        {
            var result = await mediator.Send(command);
            return result ? Ok(command.Id) : BadRequest(command.Id);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteAction(int id)
        {
            var result = await mediator.Send(new DeleteActionCommand(id));
            return result ? Ok(id) : NotFound(id);
        }
    }
}
