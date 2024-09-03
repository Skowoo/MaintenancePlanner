using ActionServiceAPI.Application.Action.Commands.CreateActionCommand;
using ActionServiceAPI.Application.DataTransferObjects.Models;
using static ActionService.Application.UnitTests.DataFixtures.ActionContextMock;

namespace ActionService.Application.UnitTests.ValidatorsTests
{
    [TestClass]
    public class CreateActionCommandValidatorUnitTests
    {
        [TestMethod]
        public void BaseValidationTest()
        {
            var context = GetContextMock();
            var validator = new CreateActionCommandValidator(context);
            SparePartDto[] usedPartsList = [new() { PartId = ExistingPartId, Quantity = 5 }];
            var command = new CreateActionCommand(
                "Example Action",
                "Example Description",
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(1),
                string.Empty,
                string.Empty,
                usedPartsList);

            var result = validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors[0].ErrorMessage.Contains("not found"));
            Assert.AreEqual(nameof(CreateActionCommand.CreatedBy), result.Errors[0].PropertyName);
        }

        [DataTestMethod]
        [DataRow(true, 1, 0)]
        [DataRow(true, 1, 5)]
        [DataRow(true, 1, 10)]
        [DataRow(false, 1, 11)]
        public void CreateActionCommandValidator_UsedPartsValidationTests_CheckIfExistingPartHaveEnoughQuantity(bool expectedResult, int usedPartId, int usedPartQuantity)
        {
            var context = GetContextMock();
            var validator = new CreateActionCommandValidator(context);

            // Initialize new part only when passed Id is bigger than 0 - otherwise tests assumes that no part should be created.
            SparePartDto? usedPart = null;
            if (usedPartId > 0)
            {
                usedPart = new()
                {
                    PartId = usedPartId,
                    Quantity = usedPartQuantity
                };
            }

            var command = new CreateActionCommand(
                "Example Action",
                "Example Description",
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(1),
                ExistingEmployeeId,
                ExistingEmployeeId,
                [usedPart!]);

            var result = validator.Validate(command);

            if (expectedResult)
                Assert.IsTrue(result.IsValid);
            else
            {
                Assert.IsFalse(result.IsValid);
                Assert.AreEqual(1, result.Errors.Count);
                Assert.IsTrue(result.Errors[0].ErrorMessage.Contains("Not enough"));
                Assert.IsTrue(result.Errors[0].PropertyName.Contains(nameof(CreateActionCommand.Parts)));
            }
        }
    }
}
