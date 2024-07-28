using AutoMapper;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using Post.Host.Data.Entities;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services.Interfaces;
using Post.Host.Services;
using Post.Host.Data;
using Infrastructure.Services;
using FluentAssertions;

namespace Post.UnitTests.Services
{
    public class ServiceTest<TEntity, TService>
        where TEntity : BaseEntity, new()
        where TService : BaseDataService<ApplicationDbContext>, IService<TEntity>
    {
        protected readonly ServiceObject<TEntity, TService> _service;

        protected readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        protected readonly Mock<ILogger<TService>> _logger;
        protected readonly Mock<IRepository<TEntity>> _repository;
        protected readonly Mock<IMapper> _mapper;

        public ServiceTest()
        {
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<TService>>();
            _repository = new Mock<IRepository<TEntity>>();
            _mapper = new Mock<IMapper>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();

            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);
            _service = new ServiceObject<TEntity, TService>(_dbContextWrapper.Object, _logger.Object, _mapper.Object, _repository.Object);
        }

        public async Task AddAsync_Success_Test()
        {
            var entity = new TEntity();
            int id = 2;

            _repository.Setup(x => x.AddAsync(It.IsAny<TEntity>())).ReturnsAsync(id);

            var result = await _service.Service.AddAsync(entity);

            result.Should().NotBeNull();
            result.Should().Be(id);
        }

        public async Task AddAsync_Failed_Test()
        {
            TEntity? entity = null;
            int? id = null;

            _repository.Setup(x => x.AddAsync(It.IsAny<TEntity>())).ReturnsAsync(id);

            var result = await _service.Service.AddAsync(entity);

            result.Should().BeNull();
        }

        public async Task UpdateAsync_Success_Test()
        {
            var entity = new TEntity();
            int id = 123;

            _repository.Setup(x => x.UpdateAsync(It.IsAny<TEntity>())).ReturnsAsync(id);

            var result = await _service.Service.UpdateAsync(entity);

            result.Should().NotBeNull();
            result.Should().Be(id);
        }

        public async Task UpdateAsync_Failed_Test()
        {
            TEntity? entity = null;
            int? id = null;

            _repository.Setup(x => x.UpdateAsync(It.IsAny<TEntity>())).ReturnsAsync(id);

            var result = await _service.Service.UpdateAsync(entity);

            result.Should().BeNull();
        }

        public async Task DeleteAsync_Success_Test()
        {
            var entity = new TEntity();
            int id = 123;
            string response = $"Object with id: {id} was successfully removed";

            _repository.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(response);

            string result = await _service.Service.DeleteAsync(id);

            result.Should().NotBeNull();
            result.Should().Be(response);
        }

        public async Task DeleteAsync_Failed_Test()
        {
            var entity = new TEntity();
            int id = 123;
            string? response = null;

            _repository.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(response);

            string result = await _service.Service.DeleteAsync(id);

            result.Should().BeNull();
        }
    }
}
