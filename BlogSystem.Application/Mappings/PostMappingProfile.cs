using AutoMapper;
using BlogSystem.Application.DTOs.Post;
using BlogSystem.Domain.Entities;

namespace BlogSystem.Application.Mappings;

public class PostMappingProfile : Profile
{
    public PostMappingProfile()
    {
        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.AuthorName,
                opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
            .ForMember(dest => dest.AuthorEmail,
                opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.PublishDate,
                opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.AuthorId,
                opt => opt.MapFrom(src => src.UserId));
        CreateMap<CreatePostDto, Post>();

        CreateMap<UpdatePostDto, Post>();
    }
}
