using AutoMapper;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using Post.Host.Data.Entities;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services.Interfaces;
using Post.Host.Data;
using Infrastructure.Services;
using FluentAssertions;
using Post.Host.Models.Dtos;

namespace Post.UnitTests.Services
{
    public class ServiceTest<TEntity, TDto, TService>
        where TEntity : BaseEntity, new()
        where TDto : BaseDto, new()
        where TService : BaseDataService<ApplicationDbContext>, IService<TEntity, TDto>
    {
        protected readonly Mock<IDbContextWrapper<ApplicationDbContext>> DbContextWrapper;
        protected readonly Mock<ILogger<TService>> Logger;
        protected readonly Mock<IRepository<TEntity>> BaseRepository;
        protected readonly Mock<IMapper> Mapper;
        protected readonly Mock<TService> ServiceMock;
        protected readonly TService Service;

        public ServiceTest(Func<IDbContextWrapper<ApplicationDbContext>, ILogger<TService>, IMapper, IRepository<TEntity>, TService> serviceFactory)
        {
            DbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            Logger = new Mock<ILogger<TService>>();
            BaseRepository = new Mock<IRepository<TEntity>>();
            Mapper = new Mock<IMapper>();
            var dbContextTransaction = new Mock<IDbContextTransaction>();

            DbContextWrapper
                .Setup(s => s.BeginTransactionAsync(CancellationToken.None))
                .ReturnsAsync(dbContextTransaction.Object);

            Service = serviceFactory(DbContextWrapper.Object, Logger.Object, Mapper.Object, BaseRepository.Object);
            ServiceMock = new Mock<TService>();
        }

        public async Task GetByIdAsync_Success_Test()
        {
            var entity = new TEntity();
            var dto = new TDto();
            int id = 2;

            BaseRepository.Setup(x => x.GetByIdAsync(
                It.Is<int>(x => x == id))).ReturnsAsync(entity);
            Mapper.Setup(x => x.Map<TDto>(It.IsAny<TEntity>())).Returns(dto);

            var result = await Service.GetByIdAsync(id);

            result.Should().Be(dto);
        }

        public async Task GetByIdAsync_Failed_Test()
        {
            BaseRepository.Setup(x => x.GetByIdAsync(
                It.IsAny<int>())).ReturnsAsync((TEntity)null!);

            var result = await Service.GetByIdAsync(2);

            result.Should().BeNull();
        }

        public async Task AddAsync_Success_Test()
        {
            var entity = new TEntity();
            int id = 2;

            BaseRepository.Setup(x => x.AddAsync(It.IsAny<TEntity>())).ReturnsAsync(id);

            var result = await Service.AddAsync(entity);

            result.Should().NotBeNull();
            result.Should().Be(id);
        }

        public async Task AddAsync_Failed_Test()
        {
            TEntity? entity = null;
            int? id = null;

            BaseRepository
                .Setup(x => x.AddAsync(It.IsAny<TEntity>()))
                .ReturnsAsync(id);

            var result = await Service.AddAsync(entity);

            result.Should().BeNull();
        }

        public async Task UpdateAsync_Success_Test()
        {
            var entity = new TEntity();
            int id = 123;

            BaseRepository
                .Setup(x => x.UpdateAsync(It.IsAny<TEntity>()))
                .ReturnsAsync(id);

            var result = await Service.UpdateAsync(entity);

            result.Should().NotBeNull();
            result.Should().Be(id);
        }

        public async Task UpdateAsync_Failed_Test()
        {
            TEntity? entity = null;
            int? id = null;

            BaseRepository
                .Setup(x => x.UpdateAsync(It.IsAny<TEntity>()))
                .ReturnsAsync(id);

            var result = await Service.UpdateAsync(entity);

            result.Should().BeNull();
        }

        public async Task DeleteAsync_Success_Test()
        {
            var entity = new TEntity();
            int id = 123;
            string response = $"Object with id: {id} was successfully removed";

            BaseRepository
                .Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(response);

            string result = await Service.DeleteAsync(id);

            result.Should().NotBeNull();
            result.Should().Be(response);
        }

        public async Task DeleteAsync_Failed_Test()
        {
            var entity = new TEntity();
            int id = 123;
            string? response = null;

            BaseRepository
                .Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(response);

            string result = await Service.DeleteAsync(id);

            result.Should().BeNull();
        }
    }
}
