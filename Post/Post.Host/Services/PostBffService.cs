using AutoMapper;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Order.Host.Services.Interfaces;
using Post.Host.Data;
using Post.Host.Models.Dtos;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services.Interfaces;

namespace Order.Host.Services;

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
}