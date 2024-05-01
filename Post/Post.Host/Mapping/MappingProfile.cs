using AutoMapper;
using Post.Host.Data.Entities;
using Post.Host.Models.Dtos;

namespace Post.Host.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PostItemEntity, PostItemDto>();
        CreateMap<PostCategoryEntity, PostCategoryDto>();
        CreateMap<PostCommentEntity, PostCommentDto>();
    }
}