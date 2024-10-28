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

public class PostLikeService : BaseDataService<ApplicationDbContext>, IService<PostLikeEntity, PostLikeDto>
{
    private readonly IRepository<PostLikeEntity> _baseRepository;
    private readonly IMapper _mapper;
    public PostLikeService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IMapper mapper,
		IRepository<PostLikeEntity> baseRepository)
        : base(dbContextWrapper, logger)
    {
        _mapper = mapper;
        _baseRepository = baseRepository;
    }

    public async Task<int?> AddAsync(PostLikeEntity entity)
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

    public async Task<int?> UpdateAsync(PostLikeEntity entity)
    {
        return await ExecuteSafeAsync(async () =>
        {
            return await _baseRepository.UpdateAsync(entity);
        });
    }

    public async Task<PostLikeDto?> GetByIdAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = _mapper.Map<PostLikeDto>(await _baseRepository.GetByIdAsync(id));
            return result;
        });
    }
}