using AutoMapper;
using Catalog.Host.Models.Requests;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Post.Host.Data;
using Post.Host.Models.Dtos;
using Post.Host.Models.Response;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services.Interfaces;

namespace Post.Host.Services;

public class PostBffService : BaseDataService<ApplicationDbContext>, IPostBffService
{
    private readonly IMapper _mapper;
    private readonly IPostBffRepository _postBffRepository;

    public PostBffService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IMapper mapper,
        IPostBffRepository postBffRepository)
        : base(dbContextWrapper, logger)
    {
        _postBffRepository = postBffRepository;
        _mapper = mapper;
    }

    public async Task<List<PostItemDto>?> GetPostsByUserNameAsync(string userName)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _postBffRepository.GetPostItemsByUserName(userName);

            if (result == null)
                return null;

            return _mapper.Map<List<PostItemDto>>(result).ToList();
        });
    }

    public async Task<List<PostItemDto>?> GetPostsByUserIdAsync(string userId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _postBffRepository.GetPostsByUserIdAsync(userId);

            if (result == null)
                return null;

			return _mapper.Map<List<PostItemDto>>(result).ToList();
		});
    }

	public async Task<PostItemDto?> GetPostByIdAsync(int id)
	{
		return await ExecuteSafeAsync(async () =>
		{
			var result = await _postBffRepository.GetPostByIdAsync(id);

            if (result == null)
                return null;

            return _mapper.Map<PostItemDto?>(result);
		});
	}

    public async Task<PaginatedResponse<PostItemDto>?> GetPostByPageAsync(PageItemRequest pageItemRequest)
    {
        return await ExecuteSafeAsync(async () =>
		{
            var result = await _postBffRepository.GetByPageAsync(pageItemRequest);

            if (result == null)
                return null;

            return new PaginatedResponse<PostItemDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(s => _mapper.Map<PostItemDto>(s)).ToList(),
                SearchByTitle = pageItemRequest.SearchByTitle,
                PageIndex = pageItemRequest.PageIndex,
                PageSize = pageItemRequest.PageSize,
                SearchByUserName = pageItemRequest.SearchByUserName,
            };
        });
    }

    public async Task<List<PostCategoryDto>?> GetPostCategoriesAsync()
    {
        return await ExecuteSafeAsync(async () =>
		{
            var result = await _postBffRepository.GetPostCategoriesAsync();

            if (result == null)
                return null;

            return _mapper.Map<List<PostCategoryDto>>(result).ToList();
        });
    }
}