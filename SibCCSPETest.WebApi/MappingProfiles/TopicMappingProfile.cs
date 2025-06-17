using AutoMapper;
using SibCCSPETest.Data;

namespace SibCCSPETest.WebApi.MappingProfiles
{
    public class TopicMappingProfile : Profile
    {
        public TopicMappingProfile()
        {
            CreateMap<Topic, TopicDTO>().ReverseMap();
            CreateMap<TopicCreateDTO, Topic>();
        }
    }
}
