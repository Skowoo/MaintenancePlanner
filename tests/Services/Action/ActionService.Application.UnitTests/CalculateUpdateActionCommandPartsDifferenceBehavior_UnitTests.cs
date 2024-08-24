using ActionServiceAPI.Application.Action.Commands.UpdateActionCommand;
using ActionServiceAPI.Application.Behaviors;
using ActionServiceAPI.Domain.Models;
using System.Reflection;

namespace ActionService.Application.UnitTests
{
    [TestClass]
    public class CalculateUpdateActionCommandPartsDifferenceBehavior_UnitTests
    {
        static List<UsedPart> Act(List<UsedPart> originalList, List<UsedPart> updatedList)
        {
            MethodInfo method = typeof(UpdateActionCommandCalculatePartsDifferenceBehavior<UpdateActionCommand, bool>)
                .GetMethod("CalculateDifference", BindingFlags.NonPublic | BindingFlags.Static)!;
            return (List<UsedPart>)method.Invoke(typeof(UpdateActionCommandCalculatePartsDifferenceBehavior<UpdateActionCommand, bool>)
                , [originalList, updatedList])!;
        }

        [TestMethod]
        public void EmptyInputs_ReturnsEmptyDifference()
        {
            List<UsedPart> originalList = [];
            List<UsedPart> updatedList = [];

            var result = Act(originalList, updatedList);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void SameInputs_ReturnsEmptyDifference()
        {
            List<UsedPart> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<UsedPart> updatedList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];

            var result = Act(originalList, updatedList);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void SingleItemsInputs_ReturnsNegativeDifference()
        {
            List<UsedPart> originalList =
            [
                new() { PartId = 1, Quantity = 10 }
            ];
            List<UsedPart> updatedList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];

            var result = Act(originalList, updatedList);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].PartId);
            Assert.AreEqual(-5, result[0].Quantity);
        }

        [TestMethod]
        public void SingleItemsInputs_ReturnsPositiveDifference()
        {
            List<UsedPart> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<UsedPart> updatedList =
            [
                new() { PartId = 1, Quantity = 10 }
            ];

            var result = Act(originalList, updatedList);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].PartId);
            Assert.AreEqual(5, result[0].Quantity);
        }

        [TestMethod]
        public void SingleItemOriginalEmptyUpdated_ReturnsNegativeDifference()
        {
            List<UsedPart> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<UsedPart> updatedList = [];

            var result = Act(originalList, updatedList);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].PartId);
            Assert.AreEqual(-5, result[0].Quantity);
        }

        [TestMethod]
        public void EmptyOriginalSingleItemUpdated_ReturnsPositiveDifference()
        {
            List<UsedPart> originalList = [];
            List<UsedPart> updatedList = 
            [
                new() { PartId = 1, Quantity = 5 }
            ];

            var result = Act(originalList, updatedList);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].PartId);
            Assert.AreEqual(5, result[0].Quantity);
        }

        [TestMethod]
        public void DoubleOriginalSingleItemUpdated_ReturnsNegativeDifference()
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

            var result = Act(originalList, updatedList);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result[0].PartId);
            Assert.AreEqual(-5, result[0].Quantity);
        }

        [TestMethod]
        public void SingleOriginalDoubleItemUpdated_ReturnsNegativeDifference()
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

            var result = Act(originalList, updatedList);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result[0].PartId);
            Assert.AreEqual(5, result[0].Quantity);
        }

        [TestMethod]
        public void SingleItemsInputsWithDifferentIds_ReturnsDoubleDifference()
        {
            List<UsedPart> originalList =
            [
                new() { PartId = 1, Quantity = 5 }
            ];
            List<UsedPart> updatedList =
            [
                new() { PartId = 2, Quantity = 5 }
            ];

            var result = Act(originalList, updatedList);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].PartId);
            Assert.AreEqual(-5, result[0].Quantity);
            Assert.AreEqual(2, result[1].PartId);            
            Assert.AreEqual(5, result[1].Quantity);
        }
    }
}
