using AutoMapper;
using BlogSystem.Application.DTOs.Post;
using BlogSystem.Application.DTOs.Shared;
using BlogSystem.Web.Models.Post;
using BlogSystem.Web.Models.Shared;

namespace BlogSystem.Web.Mappings;

public class PostMappingProfile : Profile
{
    public PostMappingProfile()
    {
        CreateMap<CreatePostViewModel, CreatePostDto>();
        CreateMap<EditPostViewModel, UpdatePostDto>();

        CreateMap<PostDto, PostDetailsViewModel>();
        CreateMap<PostDto, EditPostViewModel>();
        CreateMap<PostDto, PostItemViewModel>();

        CreateMap<PagedResultDto<PostDto>, PostListViewModel>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.Pagination, opt => opt.MapFrom(src => new PaginationViewModel
            {
                Page = src.Page,
                PageSize = src.PageSize,
                TotalCount = src.TotalCount
            }));
    }
}
