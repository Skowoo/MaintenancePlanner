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

            RuleFor(x => x.Id)
                .Must(x => context.Actions.Any(action => action.Id == x))
                .WithMessage($"Action with given id not found");

            When(x => x.GetNewUsedPartsList().Count != 0, () =>
            {
                RuleForEach(x => x.GetNewUsedPartsList()).ChildRules(part =>
                {
                    part.RuleFor(inputPart => inputPart)
                        .Must(inputPart =>
                        {
                            var dbPart = context.AvailableParts.SingleOrDefault(x => x.PartId == inputPart.PartId);
                            return dbPart != null && dbPart.Quantity >= inputPart.Quantity;
                        })
                        .When(inputPart => context.AvailableParts.Any(dbPart => dbPart.PartId == inputPart.PartId))
                        .WithMessage("Not enough parts in stock!");
                })
                .OverridePropertyName(nameof(UpdateActionCommand.Parts));
            });
        }
    }
}
