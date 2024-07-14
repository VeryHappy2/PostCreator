using AutoMapper;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Post.Host.Data;
using Post.Host.Data.Entities;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services.Interfaces;

namespace Post.Host.Services;

public class PostCategoryService : BaseDataService<ApplicationDbContext>, IService<PostCategoryEntity>
{
    private readonly IDbContextWrapper<ApplicationDbContext> _dbContextWrapper;
    private readonly IRepository<PostCategoryEntity> _repository;

    public PostCategoryService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IMapper mapper,
		IRepository<PostCategoryEntity> repository)
        : base(dbContextWrapper, logger)
    {
        _dbContextWrapper = dbContextWrapper;
        _repository = repository;
    }

    public async Task<int?> AddAsync(PostCategoryEntity entity)
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

    public async Task<int?> UpdateAsync(PostCategoryEntity entity)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return await _repository.UpdateAsync(entity);
        });
    }
}