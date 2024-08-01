using ActionServiceAPI.Application.Interfaces.DataRepositories;
using FluentValidation;

namespace ActionServiceAPI.Application.Action.Commands.Common
{
    public class ActionCommandBaseValidator : AbstractValidator<ActionCommandBase>
    {
        public ActionCommandBaseValidator(IActionContext context)
        {
            RuleFor(x => x.CreatedBy)
                .Must(id => context.Employees.Any(x => x.UserId == id))
                .WithMessage("Creator not found in database!");

            When(x => !string.IsNullOrEmpty(x.ConductedBy), () =>
            {
                RuleFor(x => x.ConductedBy)
                    .Must(id => context.Employees.Any(x => x.UserId == id))
                    .WithMessage("Employee not found in database!");
            });

            When(x => x.Parts.Any(), () =>
            {
                RuleForEach(x => x.Parts).ChildRules(parts =>
                {
                    parts.RuleFor(inputPart => inputPart.PartId)
                        .Must(partId => context.UsedParts.Any(dbPart => dbPart.PartId == partId))
                        .WithMessage("Part not found in database!");

                    parts.RuleFor(inputPart => inputPart)
                        .Must(inputPart =>
                        {
                            var dbPart = context.UsedParts.SingleOrDefault(x => x.PartId == inputPart.PartId);
                            return dbPart != null && dbPart.Quantity >= inputPart.Quantity;
                        })
                        .WithMessage("Not enough parts in stock!");
                });
            });
        }
    }
}
