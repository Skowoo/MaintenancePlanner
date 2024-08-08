using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ActionServiceAPI.Application.Action.Commands.CreateActionCommand;
using ActionServiceAPI.Application.Action.Commands.UpdateActionCommand;
using ActionServiceAPI.Application.Action.Commands.DeleteActionCommand;
using ActionServiceAPI.Application.Action.Queries.GetActionById;
using ActionServiceAPI.Application.Action.Queries.GetAllActions;
using ActionServiceAPI.Application.Interfaces.DataRepositories;

namespace ActionServiceAPI.Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class ActionController(IMediator mediator, IActionContext ctx) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(ctx.UsedParts.ToList()); // Refactor - remove diagnostic endpoint

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllActions() 
            => Ok(await mediator.Send(new GetAllActionsQuery()));

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetActionById(int id)
        {
            var result = await mediator.Send(new GetActionByIdQuery(id));
            return result is not null ? Ok(result) : NotFound(id);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAction([FromBody] CreateActionCommand command) 
            => Ok(await mediator.Send(command));

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
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
