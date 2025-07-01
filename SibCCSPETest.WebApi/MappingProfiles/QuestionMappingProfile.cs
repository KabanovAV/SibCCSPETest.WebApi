using AutoMapper;
using SibCCSPETest.Data;

namespace SibCCSPETest.WebApi.MappingProfiles
{
    public class QuestionMappingProfile : Profile
    {
        public QuestionMappingProfile()
        {
            CreateMap<Question, QuestionDTO>().ForMember(dest => dest.AnswerItems, opt => opt.MapFrom(src => src.Answers))
                .ReverseMap();
            CreateMap<QuestionCreateDTO, Question>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.AnswerItems));
        }
    }
}
