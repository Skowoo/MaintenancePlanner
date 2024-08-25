using ActionServiceAPI.Application.Action.Commands.UpdateActionCommand;
using ActionServiceAPI.Application.Behaviors;
using ActionServiceAPI.Domain.Models;
using System.Reflection;

namespace ActionService.Application.UnitTests
{
    [TestClass]
    public class CalculateUpdateActionCommandPartsDifferenceBehavior_UnitTests
    {
        static (List<UsedPart> NewUsedParts, List<UsedPart> ReturnedParts) Act(List<UsedPart> originalList, List<UsedPart> updatedList)
        {
            MethodInfo method = typeof(UpdateActionCommandCalculatePartsDifferenceBehavior<UpdateActionCommand, bool>)
                .GetMethod("CalculateDifference", BindingFlags.NonPublic | BindingFlags.Static)!;
            return ((List<UsedPart> NewUsedParts, List<UsedPart> ReturnedParts))method.Invoke(typeof(UpdateActionCommandCalculatePartsDifferenceBehavior<UpdateActionCommand, bool>)
                , [originalList, updatedList])!;
        }

        [TestMethod]
        public void EmptyInputs_ReturnsEmptyDifferenceLists()
        {
            List<UsedPart> originalList = [];
            List<UsedPart> updatedList = [];

            var (NewUsedParts, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(0, NewUsedParts.Count);
            Assert.AreEqual(0, ReturnedParts.Count);
        }

        [TestMethod]
        public void SameInputs_ReturnsEmptyDifferenceLists()
        {
            List<UsedPart> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<UsedPart> updatedList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];

            var (NewUsedParts, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(0, NewUsedParts.Count);
            Assert.AreEqual(0, ReturnedParts.Count);
        }

        [TestMethod]
        public void SingleItemsInputs_ReturnsEmptyNewPartsListsAndSingleReturnedPart()
        {
            List<UsedPart> originalList =
            [
                new() { PartId = 1, Quantity = 10 }
            ];
            List<UsedPart> updatedList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];

            var (NewUsedParts, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(0, NewUsedParts.Count);
            Assert.AreEqual(1, ReturnedParts.Count);
            Assert.AreEqual(1, ReturnedParts[0].PartId);
            Assert.AreEqual(5, ReturnedParts[0].Quantity);
        }

        [TestMethod]
        public void SingleItemsInputs_ReturnsSingleNewPartAndEmptyReturnedPart()
        {
            List<UsedPart> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<UsedPart> updatedList =
            [
                new() { PartId = 1, Quantity = 10 }
            ];

            var (NewUsedParts, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(1, NewUsedParts.Count);
            Assert.AreEqual(0, ReturnedParts.Count);
            Assert.AreEqual(1, NewUsedParts[0].PartId);
            Assert.AreEqual(5, NewUsedParts[0].Quantity);
        }

        [TestMethod]
        public void SingleItemOriginalEmptyUpdated_ReturnsEmptyNewPartsListsAndSingleReturnedPart()
        {
            List<UsedPart> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<UsedPart> updatedList = [];

            var (NewUsedParts, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(0, NewUsedParts.Count);
            Assert.AreEqual(1, ReturnedParts.Count);
            Assert.AreEqual(1, ReturnedParts[0].PartId);
            Assert.AreEqual(5, ReturnedParts[0].Quantity);
        }

        [TestMethod]
        public void EmptyOriginalSingleItemUpdated_ReturnsSingleNewPartAndEmptyReturnedPart()
        {
            List<UsedPart> originalList = [];
            List<UsedPart> updatedList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];

            var (NewUsedParts, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(1, NewUsedParts.Count);
            Assert.AreEqual(0, ReturnedParts.Count);
            Assert.AreEqual(1, NewUsedParts[0].PartId);
            Assert.AreEqual(5, NewUsedParts[0].Quantity);
        }

        [TestMethod]
        public void DoubleOriginalSingleItemUpdated_ReturnsEmptyNewPartsListsAndSingleReturnedPart()
        {
            List<UsedPart> originalList =
            [
                new() { PartId = 1, Quantity = 5 },
                new() { PartId = 2, Quantity = 5 }
            ];
            List<UsedPart> updatedList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];

            var (NewUsedParts, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(0, NewUsedParts.Count);
            Assert.AreEqual(1, ReturnedParts.Count);
            Assert.AreEqual(2, ReturnedParts[0].PartId);
            Assert.AreEqual(5, ReturnedParts[0].Quantity);
        }

        [TestMethod]
        public void SingleOriginalDoubleItemUpdated_ReturnsSingleNewPartAndEmptyReturnedPart()
        {
            List<UsedPart> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<UsedPart> updatedList =
            [
                new() { PartId = 1, Quantity = 5 },
                new() { PartId = 2, Quantity = 5 }
            ];

            var (NewUsedParts, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(1, NewUsedParts.Count);
            Assert.AreEqual(0, ReturnedParts.Count);
            Assert.AreEqual(2, NewUsedParts[0].PartId);
            Assert.AreEqual(5, NewUsedParts[0].Quantity);
        }

        [TestMethod]
        public void SingleItemsInputsWithDifferentIds_ReturnsSingleNewPartAndSingleReturnedPart()
        {
            List<UsedPart> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<UsedPart> updatedList =
            [
                new() { PartId = 2, Quantity = 10 }
            ];

            var (NewUsedParts, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(1, NewUsedParts.Count);
            Assert.AreEqual(1, ReturnedParts.Count);
            Assert.AreEqual(2, NewUsedParts[0].PartId);
            Assert.AreEqual(10, NewUsedParts[0].Quantity);
            Assert.AreEqual(1, ReturnedParts[0].PartId);
            Assert.AreEqual(5, ReturnedParts[0].Quantity);
        }
    }
}
