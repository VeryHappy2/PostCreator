using AutoMapper;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Post.Host.Data;
using Post.Host.Data.Entities;
using Post.Host.Models.Responses;
using Post.Host.Repositories;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services.Interfaces;

namespace Post.Host.Services;

public class PostItemService : BaseDataService<ApplicationDbContext>, IService<PostItemEntity>, IPostItemService
{
    private readonly IRepository<PostItemEntity> _baseRepository;
    private readonly IPostItemRepository _postItemRepository;

    public PostItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IMapper mapper,
		IRepository<PostItemEntity> baseRepository,
        IPostItemRepository postItemRepository)
        : base(dbContextWrapper, logger)
    {
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

    public async Task<PostItemEntity?> GetByIdAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return await _baseRepository.GetByIdAsync(id);
        });
    }

    public async Task<GeneralResponse> AddViewAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            PostItemEntity postItem = await _baseRepository.GetByIdAsync(id);

            if (postItem == null)
                return new GeneralResponse(false, "Not found such post");

            postItem.Views = postItem.Views;

            var result = await _baseRepository.UpdateAsync(postItem);

            if (result == null)
            {
                return new GeneralResponse(false, "Not found such post");
            }

            return new GeneralResponse(true, "View added");
        });
    }

    public async Task<GeneralResponse> AddLikeAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var post = await _baseRepository.GetByIdAsync(id);

            if (post == null)
            {
                return new GeneralResponse(false, "Not found a post");
            }

            post = post.Likes + 1;

            var result = await _baseRepository.UpdateAsync(post);

            if (result == null)
            {
                return new GeneralResponse(false, "Like wasn't updated");
            }

            return new GeneralResponse(true, "Added the like");
        });
    }
}