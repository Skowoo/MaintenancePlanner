﻿using ActionServiceAPI.Application.Action.Commands.UpdateActionCommand;
using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ActionServiceAPI.Application.Behaviors
{
    public class UpdateActionCommandCalculatePartsDifferenceBehavior<TRequest, TResponse>(IActionContext context)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : UpdateActionCommand
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var originalAction = context.Actions.Include(x => x.Parts).SingleOrDefault(x => x.Id == request.Id);

            // If the action does not exist, continue with the request - request will be validated later
            if (originalAction is null)
                return await next();

            var originalParts = originalAction.Parts.ToList();

            var (NewUsedParts, ReturnedParts) = CalculateDifference(originalParts, request.Parts.ToList());
            request.NewUsedParts = NewUsedParts;
            request.ReturnedParts = ReturnedParts;

            return await next();
        }

        // Tested using reflection
        static (List<UsedPart> NewUsedParts, List<UsedPart> ReturnedParts) CalculateDifference(IList<UsedPart> originalList, IList<UsedPart> updatedList)
        {
            List<UsedPart> differences = [];

            List<int> allPartIds = originalList.Select(x => x.PartId).Union(updatedList.Select(x => x.PartId)).ToList();
            foreach (int Id in allPartIds)
            {
                int originalQuantity = originalList.SingleOrDefault(x => x.PartId == Id)?.Quantity ?? 0;
                int updatedQuantity = updatedList.SingleOrDefault(x => x.PartId == Id)?.Quantity ?? 0;

                differences.Add(new UsedPart { PartId = Id, Quantity = updatedQuantity - originalQuantity });
            }
            var newUsedParts = differences.Where(x => x.Quantity > 0).ToList();
            var returnedParts = differences.Where(x => x.Quantity < 0).ToList();

            foreach (var part in returnedParts)
                part.Quantity = Math.Abs(part.Quantity);

            return (newUsedParts, returnedParts);
        }
    }
}
