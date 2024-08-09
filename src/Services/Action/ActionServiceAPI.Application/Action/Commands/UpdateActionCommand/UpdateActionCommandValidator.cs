using ActionServiceAPI.Application.Action.Commands.Common;
using ActionServiceAPI.Application.Interfaces.DataRepositories;
using FluentValidation;

namespace ActionServiceAPI.Application.Action.Commands.UpdateActionCommand
{
    public class UpdateActionCommandValidator : AbstractValidator<UpdateActionCommand>
    {
        public UpdateActionCommandValidator(IActionContext context)
        {
            RuleFor(x => x.Id)
                .Must(x => context.Actions.Any(action => action.Id == x))
                .WithMessage($"Action with given id not found");

            Include(new ActionCommandBaseValidator(context));
        }
    }
}
