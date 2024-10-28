using AutoMapper;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Post.Host.Data;
using Post.Host.Data.Entities;
using Post.Host.Models.Dtos;
using Post.Host.Models.Responses;
using Post.Host.Repositories;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services.Interfaces;

namespace Post.Host.Services;

public class PostItemService : BaseDataService<ApplicationDbContext>, IService<PostItemEntity, PostItemDto>, IPostItemService
{
    private readonly IRepository<PostItemEntity> _baseRepository;
    private readonly IPostItemRepository _postItemRepository;
    private readonly IMapper _mapper;

    public PostItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IMapper mapper,
		IRepository<PostItemEntity> baseRepository,
        IPostItemRepository postItemRepository)
        : base(dbContextWrapper, logger)
    {
        _mapper = mapper;
        _postItemRepository = postItemRepository;
        _baseRepository = baseRepository;
    }

    public async Task<int?> AddAsync(PostItemEntity entity)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return await _baseRepository.AddAsync(entity);
        });
    }

    public async Task<string?> DeleteAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return await _baseRepository.DeleteAsync(id);
        });
    }

    public async Task<int?> UpdateAsync(PostItemEntity entity)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return await _baseRepository.UpdateAsync(entity);
        });
    }

    public async Task<GeneralResponse> DeleteByUserIdAsync(string userId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return await _postItemRepository.DeleteByUserIdAsync(userId);
        });
    }

    public async Task<PostItemDto?> GetByIdAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return _mapper.Map<PostItemDto>(await _postItemRepository.GetByIdAsync(id));
        });
    }

    public async Task<GeneralResponse> AddViewAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            PostItemEntity postItem = await _baseRepository.GetByIdAsync(id);

            if (postItem == null)
                return new GeneralResponse(false, "Not found such post");

            postItem.Views += 1;

            var result = await _baseRepository.UpdateAsync(postItem);

            if (result == null)
            {
                return new GeneralResponse(false, "Not found such post");
            }

            return new GeneralResponse(true, "View added");
        });
    }
}