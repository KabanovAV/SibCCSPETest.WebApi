using AutoMapper;
using SibCCSPETest.Data;

namespace SibCCSPETest.WebApi.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDTO>().ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName} {src.Surname}"))
                .ReverseMap();
            CreateMap<UserCreateDTO, User>();
        }
    }
}
