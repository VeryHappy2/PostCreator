using Post.Host.Services;
using Post.Host.Data.Entities;
using Moq;
using Post.Host.Repositories.Interfaces;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Post.Host.Data;
using AutoMapper;
using Infrastructure.Services;
using FluentAssertions;

namespace Post.UnitTests.Services
{
    public class PostItemServiceTest : ServiceTest<PostItemEntity, PostItemService>
    {
        private readonly Mock<IPostItemRepository> _postItemRepository;
        public PostItemServiceTest()
             : base((dbContextWrapper, logger, mapper, baseRepository) =>
            new PostItemService(
                dbContextWrapper,
                logger,
                mapper,
                baseRepository,
                new Mock<IPostItemRepository>().Object))
        {
            _postItemRepository = new Mock<IPostItemRepository>();
        }

        [Fact]
        public async Task AddAsync_Successs()
        {
            await AddAsync_Success_Test();
        }

        [Fact]
        public async Task GetByIdAsync_Failed()
        {
            await GetByIdAsync_Failed_Test();
        }

        [Fact]
        public async Task GetByIdAsync_Success()
        {
            await GetByIdAsync_Success_Test();
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            await AddAsync_Failed_Test();
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            await UpdateAsync_Success_Test();
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            await UpdateAsync_Failed_Test();
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            await DeleteAsync_Success_Test();
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            await DeleteAsync_Failed_Test();
        }

        [Fact]
        public async Task AddLikeAsync_Success()
        {
            PostItemEntity entity = new PostItemEntity();
            int id = 1;

            BaseRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(entity);

            BaseRepository.Setup(x => x.UpdateAsync(It.IsAny<PostItemEntity>())).ReturnsAsync(id);

            var result = await Service.AddLikeAsync(id);

            result.Should().NotBeNull();
            result.Message.Should().Be("Added the like");
            result.Flag.Should().BeTrue();
        }
    }
}
