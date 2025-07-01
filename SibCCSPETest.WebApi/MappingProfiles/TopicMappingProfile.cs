using AutoMapper;
using SibCCSPETest.Data;

namespace SibCCSPETest.WebApi.MappingProfiles
{
    public class TopicMappingProfile : Profile
    {
        public TopicMappingProfile()
        {
            CreateMap<Topic, TopicDTO>().ForMember(dest => dest.SpecializationTitle, opt =>
            {
                opt.PreCondition(src => src.Specialization != null);
                opt.MapFrom(src => src.Specialization!.Title);
            }).ReverseMap();
            CreateMap<TopicCreateDTO, Topic>();
        }
    }
}
