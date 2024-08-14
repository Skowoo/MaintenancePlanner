using ActionServiceAPI.Application.Action.Commands.CreateActionCommand;
using ActionServiceAPI.Application.Action.Commands.UpdateActionCommand;
using static ActionService.Application.UnitTests.DataFixtures.ActionContextMock;

namespace ActionService.Application.UnitTests.ValidatorsTests
{
    [TestClass]
    public class UpdateActionCommandValidatorUnitTests
    {
        [DataTestMethod]
        [DataRow(true, 1)]
        [DataRow(false, 2)]
        public void UpdateActionCommandValidator_IdValidationTests(bool expectedResult, int actionId)
        {
            var context = GetContextMock();
            var validator = new UpdateActionCommandValidator(context);
            var command = new UpdateActionCommand(
                actionId,
                "Example Action",
                "Example Description",
                DateTime.Now,
                DateTime.Now.AddDays(1),
                ExistingEmployeeName,
                ExistingEmployeeName,
                []);

            var result = validator.Validate(command);

            if (expectedResult)
            {
                Assert.IsTrue(result.IsValid);
            }
            else
            {
                Assert.IsFalse(result.IsValid);
                Assert.AreEqual(1, result.Errors.Count);
                Assert.IsTrue(result.Errors[0].ErrorMessage.Contains("not found"));
                Assert.AreEqual(nameof(UpdateActionCommand.Id), result.Errors[0].PropertyName);
            }
        }

        // Validation is done in the base class, so we can perform one test to ensure that it's included.
        // All base validators are tested in CreateActionCommandValidatorUnitTests.
        [DataTestMethod]
        [DataRow(true, ExistingEmployeeName)]
        [DataRow(false, "")]
        public void UpdateActionCommandValidator_ShouldIncludeBaseValidations(bool expectedResult, string employeeName)
        {
            var context = GetContextMock();
            var validator = new UpdateActionCommandValidator(context);
            var command = new UpdateActionCommand(
                1,
                "Example Action",
                "Example Description",
                DateTime.Now,
                DateTime.Now.AddDays(1),
                employeeName,
                ExistingEmployeeName,
                []);

            var result = validator.Validate(command);

            if (expectedResult)
                Assert.IsTrue(result.IsValid);
            else
            {
                Assert.IsFalse(result.IsValid);
                Assert.AreEqual(1, result.Errors.Count);
                Assert.IsTrue(result.Errors[0].ErrorMessage.Contains("not found"));
                Assert.AreEqual(nameof(CreateActionCommand.CreatedBy), result.Errors[0].PropertyName);
            }
        }
    }
}
