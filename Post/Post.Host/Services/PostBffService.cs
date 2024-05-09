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
    private readonly IDbContextWrapper<ApplicationDbContext> _dbContextWrapper;
    private readonly IPostBffRepository _postBffRepository;

    public PostBffService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IMapper mapper,
        IPostBffRepository postBffRepository)
        : base(dbContextWrapper, logger)
    {
        _postBffRepository = postBffRepository;
        _dbContextWrapper = dbContextWrapper;
        _mapper = mapper;
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

    public async Task<PaginatedItemsResponse<PostItemDto>?> GetPostByPageAsync(PageItemRequest pageItemRequest)
    {
        return await ExecuteSafeAsync(async () =>
		{
            var result = await _postBffRepository.GetByPageAsync(pageItemRequest);

            if (result == null)
                return null;

            return new PaginatedItemsResponse<PostItemDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(s => _mapper.Map<PostItemDto>(s)).ToList(),
                Search = pageItemRequest.Search,
                PageIndex = pageItemRequest.PageIndex,
                PageSize = pageItemRequest.PageSize,
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