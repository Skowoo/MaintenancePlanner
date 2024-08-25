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
                .WithMessage("Employee not found in database!");

            When(x => !string.IsNullOrEmpty(x.ConductedBy), () =>
            {
                RuleFor(x => x.ConductedBy)
                    .Must(id => context.Employees.Any(x => x.UserId == id))
                    .WithMessage("Employee not found in database!");
            });

            When(x => x.Parts.Any(), () =>
            {
                RuleForEach(x => x.Parts).ChildRules(part =>
                {
                    part.RuleFor(inputPart => inputPart.PartId)
                        .Must(partId => context.AvailableParts.Any(dbPart => dbPart.PartId == partId))
                        .WithMessage("Part not found in database!");
                });
            });
        }
    }
}