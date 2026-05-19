using AutoMapper;
using BlogSystem.Application.DTOs.User;
using BlogSystem.Web.Models.Auth;

namespace BlogSystem.Web.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<LoginViewModel, LoginDto>();
        CreateMap<RegisterViewModel, RegisterUserDto>();
    }
}
