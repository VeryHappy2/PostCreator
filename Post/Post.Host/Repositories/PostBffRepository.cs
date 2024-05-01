using Infrastructure.Services.Interfaces;
using Post.Host.Data;
using Post.Host.Data.Entities;
using Post.Host.Repositories.Interfaces;

namespace Post.Host.Repositories
{
    public class PostBffRepository : IPostBffRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PostBffRepository(IDbContextWrapper<ApplicationDbContext> context)
        {
            _dbContext = context.DbContext;
        }

        public async Task<PostItemEntity?> GetPostByIdAsync(int id) => await _dbContext.PostItemEntity.FindAsync(id);

        public async Task<List<PostItemEntity>?> GetPostsByUserIdAsync(string userId) => _dbContext.PostItemEntity.Where(x => x.UserId == userId).ToList();
    }
}
