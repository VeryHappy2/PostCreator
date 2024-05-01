using AutoMapper;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Post.Host.Data;
using Post.Host.Data.Entities;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services.Interfaces;

namespace Order.Host.Services;

public class PostCommentService : BaseDataService<ApplicationDbContext>, IService<PostCommentEntity>
{
    private readonly IDbContextWrapper<ApplicationDbContext> _dbContextWrapper;
    private readonly IRepository<PostCommentEntity> _repository;

    public PostCommentService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IMapper mapper,
        IRepository<PostCommentEntity> repository)
        : base(dbContextWrapper, logger)
    {
        _dbContextWrapper = dbContextWrapper;
        _repository = repository;
    }

    public async Task<int?> AddAsync(PostCommentEntity entity)
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

    public async Task<int?> UpdateAsync(int id, PostCommentEntity entity)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return await _repository.UpdateAsync(id, entity);
        });
    }
}