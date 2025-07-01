using AutoMapper;
using SibCCSPETest.Data;

namespace SibCCSPETest.WebApi.MappingProfiles
{
    public class AnswerMappingProfile : Profile
    {
        public AnswerMappingProfile()
        {
            CreateMap<Answer, AnswerDTO>().ForMember(dest => dest.QuestionTitle, opt =>
            {
                opt.PreCondition(src => src.Question != null);
                opt.MapFrom(src => src.Question!.Title);
            }).ReverseMap();
            CreateMap<AnswerCreateDTO, Answer>();
        }
    }
}
