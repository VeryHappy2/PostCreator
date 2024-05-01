using AutoMapper;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Post.Host.Data;
using Post.Host.Data.Entities;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services.Interfaces;

namespace Post.Host.Services;

public class PostItemService : BaseDataService<ApplicationDbContext>, IService<PostItemEntity>
{
    private readonly IMapper _mapper;
    private readonly IDbContextWrapper<ApplicationDbContext> _dbContextWrapper;
    private readonly IRepository<PostItemEntity> _repository;

    public PostItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IMapper mapper,
		IRepository<PostItemEntity> repository)
        : base(dbContextWrapper, logger)
    {
        _dbContextWrapper = dbContextWrapper;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<int?> AddAsync(PostItemEntity entity)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return await _repository.AddAsync(entity);
        });
    }

    public async Task<string?> DeleteAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return await _repository.DeleteAsync(id);
        });
    }

    public async Task<int?> UpdateAsync(PostItemEntity entity)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return await _repository.UpdateAsync(entity);
        });
    }
}