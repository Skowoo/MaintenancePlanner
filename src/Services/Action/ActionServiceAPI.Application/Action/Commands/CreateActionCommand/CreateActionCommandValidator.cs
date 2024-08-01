using ActionServiceAPI.Application.Action.Commands.Common;
using ActionServiceAPI.Application.Interfaces.DataRepositories;
using FluentValidation;

namespace ActionServiceAPI.Application.Action.Commands.CreateActionCommand
{
    public class CreateActionCommandValidator : AbstractValidator<CreateActionCommand>
    {
        public CreateActionCommandValidator(IActionContext context)
        {
            Include(new ActionCommandBaseValidator(context));
        }
    }
}
