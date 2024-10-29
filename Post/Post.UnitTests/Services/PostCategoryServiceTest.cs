using Post.Host.Data.Entities;
using Post.Host.Models.Dtos;
using Post.Host.Services;

namespace Post.UnitTests.Services
{
    public class PostCategoryServiceTest : ServiceTest<PostCategoryEntity, PostCategoryDto, PostCategoryService>
    {
        public PostCategoryServiceTest() : base((dbContextWrapper, logger, mapper, baseRepository) =>
            new PostCategoryService(
                dbContextWrapper,
                logger,
                mapper,
                baseRepository))
        {
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
        public async Task AddAsync_Success()
        {
            await AddAsync_Success_Test();
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
