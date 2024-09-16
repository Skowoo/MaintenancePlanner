using ActionServiceAPI.Application.DataTransferObjects.Models;
using ActionServiceAPI.Domain.Models;
using AutoMapper;

namespace ActionServiceAPI.Application.DataTransferObjects.Mappings
{
    public class ActionMappingProfile : Profile
    {
        public ActionMappingProfile()
        {
            CreateMap<ActionEntity, ActionDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.UserId))
                .ForMember(dest => dest.ConductedBy, opt => opt.MapFrom(src => (src.ConductedBy != null) ? src.ConductedBy.UserId : ""))
                .ForMember(dest => dest.Parts, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.Parts.Select(p => context.Mapper.Map<UsedPart>(p)).ToList()));
        }
    }
}