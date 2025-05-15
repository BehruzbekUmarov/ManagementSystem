using AutoMapper;
using InnerSystem.Identity.DTOs.Users;
using InnerSystem.Identity.Models;

namespace InnerSystem.Identity.Abstract;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
		CreateMap<User, UserDto>()
			.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));
		CreateMap<CreateUserDto, User>().ReverseMap();
		CreateMap<UpdateUserDto, User>().ReverseMap();
	}
}
