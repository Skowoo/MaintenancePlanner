using AutoMapper;
using IdentityServiceAPI.Models;

namespace IdentityServiceAPI.Mapping
{
    public class ApplicationUserMappingProfile : Profile
    {
        public ApplicationUserMappingProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserExternalModel>();
        }
    }
}
