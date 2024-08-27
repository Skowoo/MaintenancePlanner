using ActionServiceAPI.Application.DataTransferObjects.Models;
using ActionServiceAPI.Domain.Models;
using AutoMapper;

namespace ActionServiceAPI.Application.DataTransferObjects.Mappings
{
    public class SparePartMappingProfile : Profile
    {
        public SparePartMappingProfile()
        {
            CreateMap<SparePartDto, UsedPart>();
            CreateMap<SparePartDto, AvailablePart>();

            CreateMap<UsedPart, SparePartDto>();
            CreateMap<AvailablePart, SparePartDto>();
        }
    }
}
