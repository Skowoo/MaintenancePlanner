using ActionServiceAPI.Application.Action.Commands.UpdateActionCommand;
using ActionServiceAPI.Application.Behaviors;
using ActionServiceAPI.Application.DataTransferObjects.Models;
using System.Reflection;

namespace ActionService.Application.UnitTests.BehaviorsLogicTests
{
    [TestClass]
    public class CalculateUpdateActionCommandPartsDifferenceBehavior_UnitTests
    {
        static (List<SparePartDto> NewSparePartDtos, List<SparePartDto> ReturnedParts) Act(List<SparePartDto> originalList, List<SparePartDto> updatedList)
        {
            MethodInfo method = typeof(UpdateActionCommandCalculatePartsDifferenceBehavior<UpdateActionCommand, bool>)
                .GetMethod("CalculateDifference", BindingFlags.NonPublic | BindingFlags.Static)!;
            return ((List<SparePartDto> NewSparePartDtos, List<SparePartDto> ReturnedParts))method.Invoke(typeof(UpdateActionCommandCalculatePartsDifferenceBehavior<UpdateActionCommand, bool>)
                , [originalList, updatedList])!;
        }

        [TestMethod]
        public void EmptyInputs_ReturnsEmptyDifferenceLists()
        {
            List<SparePartDto> originalList = [];
            List<SparePartDto> updatedList = [];

            var (NewSparePartDtos, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(0, NewSparePartDtos.Count);
            Assert.AreEqual(0, ReturnedParts.Count);
        }

        [TestMethod]
        public void SameInputs_ReturnsEmptyDifferenceLists()
        {
            List<SparePartDto> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<SparePartDto> updatedList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];

            var (NewSparePartDtos, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(0, NewSparePartDtos.Count);
            Assert.AreEqual(0, ReturnedParts.Count);
        }

        [TestMethod]
        public void SingleItemsInputs_ReturnsEmptyNewPartsListsAndSingleReturnedPart()
        {
            List<SparePartDto> originalList =
            [
                new() { PartId = 1, Quantity = 10 }
            ];
            List<SparePartDto> updatedList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];

            var (NewSparePartDtos, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(0, NewSparePartDtos.Count);
            Assert.AreEqual(1, ReturnedParts.Count);
            Assert.AreEqual(1, ReturnedParts[0].PartId);
            Assert.AreEqual(5, ReturnedParts[0].Quantity);
        }

        [TestMethod]
        public void SingleItemsInputs_ReturnsSingleNewPartAndEmptyReturnedPart()
        {
            List<SparePartDto> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<SparePartDto> updatedList =
            [
                new() { PartId = 1, Quantity = 10 }
            ];

            var (NewSparePartDtos, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(1, NewSparePartDtos.Count);
            Assert.AreEqual(0, ReturnedParts.Count);
            Assert.AreEqual(1, NewSparePartDtos[0].PartId);
            Assert.AreEqual(5, NewSparePartDtos[0].Quantity);
        }

        [TestMethod]
        public void SingleItemOriginalEmptyUpdated_ReturnsEmptyNewPartsListsAndSingleReturnedPart()
        {
            List<SparePartDto> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<SparePartDto> updatedList = [];

            var (NewSparePartDtos, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(0, NewSparePartDtos.Count);
            Assert.AreEqual(1, ReturnedParts.Count);
            Assert.AreEqual(1, ReturnedParts[0].PartId);
            Assert.AreEqual(5, ReturnedParts[0].Quantity);
        }

        [TestMethod]
        public void EmptyOriginalSingleItemUpdated_ReturnsSingleNewPartAndEmptyReturnedPart()
        {
            List<SparePartDto> originalList = [];
            List<SparePartDto> updatedList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];

            var (NewSparePartDtos, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(1, NewSparePartDtos.Count);
            Assert.AreEqual(0, ReturnedParts.Count);
            Assert.AreEqual(1, NewSparePartDtos[0].PartId);
            Assert.AreEqual(5, NewSparePartDtos[0].Quantity);
        }

        [TestMethod]
        public void DoubleOriginalSingleItemUpdated_ReturnsEmptyNewPartsListsAndSingleReturnedPart()
        {
            List<SparePartDto> originalList =
            [
                new() { PartId = 1, Quantity = 5 },
                new() { PartId = 2, Quantity = 5 }
            ];
            List<SparePartDto> updatedList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];

            var (NewSparePartDtos, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(0, NewSparePartDtos.Count);
            Assert.AreEqual(1, ReturnedParts.Count);
            Assert.AreEqual(2, ReturnedParts[0].PartId);
            Assert.AreEqual(5, ReturnedParts[0].Quantity);
        }

        [TestMethod]
        public void SingleOriginalDoubleItemUpdated_ReturnsSingleNewPartAndEmptyReturnedPart()
        {
            List<SparePartDto> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<SparePartDto> updatedList =
            [
                new() { PartId = 1, Quantity = 5 },
                new() { PartId = 2, Quantity = 5 }
            ];

            var (NewSparePartDtos, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(1, NewSparePartDtos.Count);
            Assert.AreEqual(0, ReturnedParts.Count);
            Assert.AreEqual(2, NewSparePartDtos[0].PartId);
            Assert.AreEqual(5, NewSparePartDtos[0].Quantity);
        }

        [TestMethod]
        public void SingleItemsInputsWithDifferentIds_ReturnsSingleNewPartAndSingleReturnedPart()
        {
            List<SparePartDto> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<SparePartDto> updatedList =
            [
                new() { PartId = 2, Quantity = 10 }
            ];

            var (NewSparePartDtos, ReturnedParts) = Act(originalList, updatedList);

            Assert.AreEqual(1, NewSparePartDtos.Count);
            Assert.AreEqual(1, ReturnedParts.Count);
            Assert.AreEqual(2, NewSparePartDtos[0].PartId);
            Assert.AreEqual(10, NewSparePartDtos[0].Quantity);
            Assert.AreEqual(1, ReturnedParts[0].PartId);
            Assert.AreEqual(5, ReturnedParts[0].Quantity);
        }
    }
}
