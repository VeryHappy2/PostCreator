using Post.Host.Services;
using System;
using System.Threading.Tasks;
using Post.Host.Data.Entities;

namespace Post.UnitTests.Services
{
    public class PostCategoryServiceTest : ServiceTest<PostCategoryEntity, PostCategoryService>
    {
        public PostCategoryServiceTest() : base()
        {
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
