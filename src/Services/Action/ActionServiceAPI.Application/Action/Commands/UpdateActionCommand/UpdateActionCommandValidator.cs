using ActionServiceAPI.Application.Action.Commands.Common;
using ActionServiceAPI.Application.Interfaces.DataRepositories;
using FluentValidation;

namespace ActionServiceAPI.Application.Action.Commands.UpdateActionCommand
{
    public class UpdateActionCommandValidator : AbstractValidator<UpdateActionCommand>
    {
        public UpdateActionCommandValidator(IActionContext context)
        {
            Include(new ActionCommandBaseValidator(context));
        }
    }
}
