using Post.Host.Services;
using Post.Host.Data.Entities;

namespace Post.UnitTests.Services
{
    public class PostCommentServiceTest : ServiceTest<PostCommentEntity, PostCommentService>
    {
        public PostCommentServiceTest() : base()
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
