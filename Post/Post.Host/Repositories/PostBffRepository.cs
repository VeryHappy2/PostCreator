using Catalog.Host.Models.Requests;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Post.Host.Data;
using Post.Host.Data.Entities;
using Post.Host.Repositories.Interfaces;

namespace Post.Host.Repositories
{
    public class PostBffRepository : IPostBffRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<PostBffRepository> _logger;

        public PostBffRepository(
            IDbContextWrapper<ApplicationDbContext> context,
            ILogger<PostBffRepository> logger)
        {
            _logger = logger;
            _dbContext = context.DbContext;
        }

        public async Task<PostItemEntity?> GetPostByIdAsync(int id)
            => await _dbContext.PostItemEntity.FindAsync(id);

        public async Task<List<PostItemEntity>?> GetPostsByUserIdAsync(string userId)
            => _dbContext.PostItemEntity.Where(x => x.UserId == userId).ToList();

        public async Task<List<PostCategoryEntity>> GetPostCategoriesAsync()
            => await _dbContext.PostCategoryEntity.ToListAsync();

        public async Task<PaginatedItems<PostItemEntity>> GetByPageAsync(PageItemRequest pageItemRequest)
        {
            IQueryable<PostItemEntity> query = _dbContext.PostItemEntity;

            if (pageItemRequest.CategoryFilter.HasValue)
            {
                query = query.Where(w => w.CategoryId == pageItemRequest.CategoryFilter.Value);
            }

            if (!string.IsNullOrEmpty(pageItemRequest.Search))
            {
                pageItemRequest.Search.ToLower();
                query = query.Where(w => w.Title.ToLower().Contains(pageItemRequest.Search));
            }

            var totalItems = await query.LongCountAsync();

            var itemsOnPage = await query.OrderByDescending(c => c.Date)
            .Include(i => i.Category)
            .Include(i => i.Comments)
            .Skip(pageItemRequest.PageSize * pageItemRequest.PageIndex)
            .Take(pageItemRequest.PageSize)
            .ToListAsync();

            foreach (var item in itemsOnPage)
                _logger.LogInformation($"Data of item:" + JsonConvert.SerializeObject(item));

            return new PaginatedItems<PostItemEntity>() { TotalCount = totalItems, Data = itemsOnPage };
        }
    }
}
