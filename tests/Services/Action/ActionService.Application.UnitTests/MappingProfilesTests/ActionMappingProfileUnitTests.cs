using ActionServiceAPI.Application.DataTransferObjects.Mappings;
using ActionServiceAPI.Application.DataTransferObjects.Models;
using AutoMapper;
using static ActionService.Application.UnitTests.DataFixtures.ActionContextMock;

namespace ActionService.Application.UnitTests.MappingProfilesTests
{
    [TestClass]
    public class ActionMappingProfileUnitTests
    {
        static IMapper GetActionMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ActionMappingProfile>();
                cfg.AddProfile<SparePartMappingProfile>();
            });
            return config.CreateMapper();
        }

        [TestMethod]
        public void ShouldHaveValidMappings()
        {
            var mapper = GetActionMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void ShouldSupportMappingFromActionEntityToActionDto()
        {
            var mapper = GetActionMapper();
            var context = GetContextMock();

            var entity = context.Actions.First();
            var dto = mapper.Map<ActionDto>(entity);

            Assert.AreEqual(entity.Id, dto.Id);
            Assert.AreEqual(entity.Name, dto.Name);
            Assert.AreEqual(entity.Description, dto.Description);
            Assert.AreEqual(entity.CreatedBy.UserId, dto.CreatedBy);
            Assert.AreEqual(entity.ConductedBy?.UserId, dto.ConductedBy);
            Assert.AreEqual(entity.Parts.Count, dto.Parts.Count);
            Assert.IsTrue(entity.Parts.Count > 0);
            Assert.AreEqual(entity.Parts.Single().Quantity, dto.Parts[0].Quantity);
            Assert.AreEqual(entity.Parts.Single().PartId, dto.Parts[0].PartId);
        }
    }
}
