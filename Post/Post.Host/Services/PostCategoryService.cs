using AutoMapper;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Post.Host.Data;
using Post.Host.Data.Entities;
using Post.Host.Models.Dtos;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services.Interfaces;

namespace Post.Host.Services;

public class PostCategoryService : BaseDataService<ApplicationDbContext>, IService<PostCategoryEntity, PostCategoryDto>
{
    private readonly IRepository<PostCategoryEntity> _repository;
    private readonly IMapper _mapper;
    public PostCategoryService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IMapper mapper,
		IRepository<PostCategoryEntity> repository)
        : base(dbContextWrapper, logger)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PostCategoryDto?> GetByIdAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return _mapper.Map<PostCategoryDto>(await _repository.GetByIdAsync(id));
        });
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