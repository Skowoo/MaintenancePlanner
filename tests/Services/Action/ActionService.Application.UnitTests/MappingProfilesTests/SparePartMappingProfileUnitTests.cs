using ActionServiceAPI.Application.DataTransferObjects.Mappings;
using ActionServiceAPI.Application.DataTransferObjects.Models;
using ActionServiceAPI.Domain.Models;
using AutoMapper;

namespace ActionService.Application.UnitTests.MappingProfilesTests
{
    [TestClass]
    public class SparePartMappingProfileUnitTests
    {
        static IMapper GetActionMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
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
        public void ShouldSupportMappingFromUsedPartToSparePartDto()
        {
            UsedPart entity = new(1, 10);
            var mapper = GetActionMapper();

            var dto = mapper.Map<SparePartDto>(entity);

            Assert.AreEqual(entity.PartId, dto.PartId);
            Assert.AreEqual(entity.Quantity, dto.Quantity);
        }

        [TestMethod]
        public void ShouldSupportMappingFromAvailablePartToSparePartDto()
        {
            AvailablePart entity = new(1, 10);
            var mapper = GetActionMapper();

            var dto = mapper.Map<SparePartDto>(entity);

            Assert.AreEqual(entity.PartId, dto.PartId);
            Assert.AreEqual(entity.Quantity, dto.Quantity);
        }

        [TestMethod]
        public void ShouldSupportMappingFromSparePartDtoToUsedPart()
        {
            SparePartDto dto = new()
            {
                PartId = 1,
                Quantity = 10
            };
            var mapper = GetActionMapper();

            var entity = mapper.Map<UsedPart>(dto);

            Assert.AreEqual(dto.PartId, entity.PartId);
            Assert.AreEqual(dto.Quantity, entity.Quantity);
            Assert.AreEqual(entity.Id, 0);
        }

        [TestMethod]
        public void ShouldSupportMappingFromSparePartDtoToAvailablePart()
        {
            SparePartDto dto = new()
            {
                PartId = 1,
                Quantity = 10
            };
            var mapper = GetActionMapper();

            var entity = mapper.Map<AvailablePart>(dto);

            Assert.AreEqual(dto.PartId, entity.PartId);
            Assert.AreEqual(dto.Quantity, entity.Quantity);
            Assert.AreEqual(entity.Id, 0);
        }
    }
}
