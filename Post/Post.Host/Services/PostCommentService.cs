using AutoMapper;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Post.Host.Data;
using Post.Host.Data.Entities;
using Post.Host.Models.Dtos;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services.Interfaces;

namespace Post.Host.Services;

public class PostCommentService : BaseDataService<ApplicationDbContext>, IService<PostCommentEntity, PostCommentDto>
{
    private readonly IDbContextWrapper<ApplicationDbContext> _dbContextWrapper;
    private readonly IRepository<PostCommentEntity> _repository;
    private readonly IMapper _mapper;
    public PostCommentService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IMapper mapper,
        IRepository<PostCommentEntity> repository)
        : base(dbContextWrapper, logger)
    {
        _mapper = mapper;
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

    public async Task<int?> UpdateAsync(PostCommentEntity entity)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return await _repository.UpdateAsync(entity);
        });
    }

    public async Task<PostCommentDto?> GetByIdAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return _mapper.Map<PostCommentDto>(await _repository.GetByIdAsync(id));
        });
    }
}