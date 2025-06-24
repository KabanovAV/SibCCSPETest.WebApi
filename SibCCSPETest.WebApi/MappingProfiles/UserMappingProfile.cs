using AutoMapper;
using SibCCSPETest.Data;

namespace SibCCSPETest.WebApi.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDTO>().ForMember(dest => dest.FullName, opt => opt.MapFrom(src => 
                string.Join(" ", new[] { src.LastName, src.FirstName, src.Surname }.Where(s => !string.IsNullOrEmpty(s)))))
                .ReverseMap()
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => GetNamePart(src.FullName, 0)))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => GetNamePart(src.FullName, 1)))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => GetNamePart(src.FullName, 2)));
            CreateMap<UserCreateDTO, User>();
        }

        private static string GetNamePart(string fullName, int index)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return string.Empty;
            var names = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return names.Length > index ? names[index] : string.Empty;
        }
    }
}
