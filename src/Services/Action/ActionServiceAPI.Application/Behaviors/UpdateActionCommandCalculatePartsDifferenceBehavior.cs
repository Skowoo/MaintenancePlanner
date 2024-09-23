using ActionServiceAPI.Application.Action.Commands.UpdateActionCommand;
using ActionServiceAPI.Application.DataTransferObjects.Models;
using ActionServiceAPI.Application.Interfaces.DataRepositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ActionServiceAPI.Application.Behaviors
{
    public class UpdateActionCommandCalculatePartsDifferenceBehavior<TRequest, TResponse>(IActionContext context, IMapper mapper)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : UpdateActionCommand
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var originalAction = context.Actions.Include(x => x.Parts).SingleOrDefault(x => x.Id == request.Id);

            // If the action does not exist, continue with the request - request will be validated later
            if (originalAction is null)
                return await next();

            var originalParts = originalAction.Parts.ToList();

            var (NewUsedParts, ReturnedParts) = CalculateDifference(originalParts.Select(mapper.Map<SparePartDto>).ToList(), request.Parts.ToList());
            request.SetUsedPartsList(NewUsedParts);
            request.SetReturnedPartsList(ReturnedParts);

            return await next();
        }

        // Tested using reflection
        static (List<SparePartDto> NewUsedParts, List<SparePartDto> ReturnedParts) CalculateDifference(IList<SparePartDto> originalList, IList<SparePartDto> updatedList)
        {
            List<SparePartDto> differences = [],
                newUsedParts = [],
                returnedParts = [];

            List<int> allPartIds = originalList.Select(x => x.PartId).Union(updatedList.Select(x => x.PartId)).ToList();
            foreach (int Id in allPartIds)
            {
                int originalQuantity = originalList.SingleOrDefault(x => x.PartId == Id)?.Quantity ?? 0;
                int updatedQuantity = updatedList.SingleOrDefault(x => x.PartId == Id)?.Quantity ?? 0;

                int difference = updatedQuantity - originalQuantity;

                if (difference > 0)
                    newUsedParts.Add(new SparePartDto { PartId = Id, Quantity = difference });
                else if (difference < 0)
                    returnedParts.Add(new SparePartDto { PartId = Id, Quantity = Math.Abs(difference) });
            }

            return (newUsedParts, returnedParts);
        }
    }
}
