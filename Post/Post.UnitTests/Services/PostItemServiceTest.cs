using Post.Host.Services;
using Post.Host.Data.Entities;
using Moq;
using Post.Host.Repositories.Interfaces;
using Post.Host.Models.Dtos;

namespace Post.UnitTests.Services
{
    public class PostItemServiceTest : ServiceTest<PostItemEntity, PostItemDto, PostItemService>
    {
        public PostItemServiceTest()
             : base((dbContextWrapper, logger, mapper, baseRepository) =>
            new PostItemService(
                dbContextWrapper,
                logger,
                mapper,
                baseRepository,
                new Mock<IPostItemRepository>().Object))
        {
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
    }
}
