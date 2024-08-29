using ActionServiceAPI.Application.DataTransferObjects.Models;
using ActionServiceAPI.Domain.Models;
using AutoMapper;

namespace ActionServiceAPI.Application.DataTransferObjects.Mappings
{
    public class SparePartMappingProfile : Profile
    {
        public SparePartMappingProfile()
        {
            CreateMap<SparePartDto, UsedPart>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<SparePartDto, AvailablePart>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UsedPart, SparePartDto>();
            CreateMap<AvailablePart, SparePartDto>();
        }
    }
}
