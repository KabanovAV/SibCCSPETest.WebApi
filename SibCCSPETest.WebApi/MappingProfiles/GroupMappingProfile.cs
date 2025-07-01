using AutoMapper;
using SibCCSPETest.Data;

namespace SibCCSPETest.WebApi.MappingProfiles
{
    public class GroupMappingProfile : Profile
    {
        public GroupMappingProfile()
        {
            CreateMap<Group, GroupDTO>().ForMember(dest => dest.SpecializationTitle, opt =>
            {
                opt.PreCondition(src => src.Specialization != null);
                opt.MapFrom(src => src.Specialization!.Title);
            }).ReverseMap();
            CreateMap<GroupCreateDTO, Group>();
        }
    }
}
