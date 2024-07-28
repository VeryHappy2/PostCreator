using AutoMapper;
using Infrastructure.Services.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Post.Host.Data.Entities;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services.Interfaces;
using Post.Host.Data;

namespace Post.UnitTests.Services
{
    public class ServiceObject<TEntity, TService>
      where TEntity : BaseEntity
      where TService : BaseDataService<ApplicationDbContext>, IService<TEntity>
    {
        public readonly IService<TEntity> Service;

        public ServiceObject(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<TService> logger,
            IMapper mapper,
            IRepository<TEntity> repository)
        {
            Service = (IService<TEntity>)Activator.CreateInstance(typeof(TService), dbContextWrapper, logger, mapper, repository);
        }
    }
}
