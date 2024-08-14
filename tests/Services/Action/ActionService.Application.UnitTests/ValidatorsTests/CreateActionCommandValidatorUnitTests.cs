﻿using ActionServiceAPI.Application.Action.Commands.CreateActionCommand;
using ActionServiceAPI.Domain.Models;
using static ActionService.Application.UnitTests.DataFixtures.ActionContextMock;

namespace ActionService.Application.UnitTests.ValidatorsTests
{
    [TestClass]
    public class CreateActionCommandValidatorUnitTests
    {
        [DataTestMethod]        
        [DataRow(true, ExistingEmployeeName)]
        [DataRow(false, "")]
        [DataRow(false, "Not existing employee")]
        public void CreateActionCommandValidator_CreatedByValidationTests(bool expectedResult, string createdByEmployeeName)
        {
            var context = GetContextMock();
            var validator = new CreateActionCommandValidator(context);
            UsedPart[] usedPartsList = [new() { PartId = ExistingPartId, Quantity = 5 }];
            var command = new CreateActionCommand(
                "Example Action",
                "Example Description",
                DateTime.Now,
                DateTime.Now.AddDays(1),
                createdByEmployeeName,
                string.Empty,
                usedPartsList);

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

        [DataTestMethod]
        [DataRow(true, ExistingEmployeeName)]
        [DataRow(true, "")] // Empty value is acceptable for ConductedBy
        [DataRow(false, "Not existing employee")]
        public void CreateActionCommandValidator_ConductedByValidationTests(bool expectedResult, string conductedByEmployeeName)
        {
            var context = GetContextMock();
            var validator = new CreateActionCommandValidator(context);
            UsedPart[] usedPartsList = [new() { PartId = ExistingPartId, Quantity = 5 }];
            var command = new CreateActionCommand(
                "Example Action",
                "Example Description",
                DateTime.Now,
                DateTime.Now.AddDays(1),
                ExistingEmployeeName,
                conductedByEmployeeName,
                usedPartsList);

            var result = validator.Validate(command);

            if (expectedResult)
                Assert.IsTrue(result.IsValid);
            else
            {
                Assert.IsFalse(result.IsValid);
                Assert.AreEqual(1, result.Errors.Count);
                Assert.IsTrue(result.Errors[0].ErrorMessage.Contains("not found"));
                Assert.AreEqual(nameof(CreateActionCommand.ConductedBy), result.Errors[0].PropertyName);
            }
        }

        [DataTestMethod]
        [DataRow(true, -1, 0)]
        [DataRow(true, 1, 5)]
        [DataRow(false, 2, 5)]
        public void CreateActionCommandValidator_UsedPartsValidationTests_CheckIfPartsExistsInDatabase(bool expectedResult, int usedPartId, int usedPartQuantity)
        {
            var context = GetContextMock();
            var validator = new CreateActionCommandValidator(context);

            // Initialize new part only when passed Id is bigger than 0 - otherwise tests assumes that no part should be created.
            UsedPart? usedPart = null;
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
                DateTime.Now,
                DateTime.Now.AddDays(1),
                ExistingEmployeeName,
                ExistingEmployeeName,
                [usedPart!]);

            var result = validator.Validate(command);

            if (expectedResult)
                Assert.IsTrue(result.IsValid);
            else
            {
                Assert.IsFalse(result.IsValid);
                Assert.AreEqual(1, result.Errors.Count);
                Assert.IsTrue(result.Errors[0].ErrorMessage.Contains("not found"));
                Assert.IsTrue(result.Errors[0].PropertyName.Contains(nameof(CreateActionCommand.Parts)));
            }
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
            UsedPart? usedPart = null;
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
                DateTime.Now,
                DateTime.Now.AddDays(1),
                ExistingEmployeeName,
                ExistingEmployeeName,
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
