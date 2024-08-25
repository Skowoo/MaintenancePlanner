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

            When(x => x.Parts.Any(), () =>
            {
                RuleForEach(x => x.Parts).ChildRules(part =>
                {
                    part.RuleFor(inputPart => inputPart)
                        .Must(inputPart =>
                        {
                            var dbPart = context.AvailableParts.SingleOrDefault(x => x.PartId == inputPart.PartId);
                            return dbPart != null && dbPart.Quantity >= inputPart.Quantity;
                        })
                        .When(inputPart => context.AvailableParts.Any(dbPart => dbPart.PartId == inputPart.PartId))
                        .WithMessage("Not enough parts in stock!");
                });
            });
        }
    }
}
