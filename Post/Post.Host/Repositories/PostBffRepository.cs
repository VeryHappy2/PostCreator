using Catalog.Host.Models.Requests;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<PostItemEntity>?> GetPostsByUserIdAsync(string userId)
        {
            return await _dbContext.PostItemEntity
                .Include(x => x.Category)
                .Include(y => y.Comments)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<PostCategoryEntity>> GetPostCategoriesAsync()
            => await _dbContext.PostCategoryEntity.ToListAsync();

        public async Task<PaginatedItems<PostItemEntity>> GetByPageAsync(PageItemRequest pageItemRequest)
        {
            IQueryable<PostItemEntity> query = _dbContext.PostItemEntity;

            if (pageItemRequest.CategoryFilter.HasValue)
            {
                query = query.Where(w => w.CategoryId == pageItemRequest.CategoryFilter.Value);
            }

            if (!string.IsNullOrEmpty(pageItemRequest.SearchByTitle))
            {
                pageItemRequest.SearchByTitle.ToLower();
                query = query.Where(w => w.Title.ToLower().Contains(pageItemRequest.SearchByTitle));
            }

            if (!string.IsNullOrEmpty(pageItemRequest.SearchByUserName))
            {
                pageItemRequest.SearchByUserName.ToLower();
                query = query.Where(w => w.UserName.ToLower().Contains(pageItemRequest.SearchByUserName));
            }

            var totalItems = await query.LongCountAsync();

            var itemsOnPage = await query
                .OrderByDescending(p => p.Views > 0 ? (p.Comments.Count + p.Likes.Count) / p.Views : 0)
                .ThenByDescending(p => p.Date)
                .Include(i => i.Category)
                .Include(i => i.Comments)
                .Include(i => i.Likes)
                .Skip(pageItemRequest.PageSize * pageItemRequest.PageIndex)
                .Take(pageItemRequest.PageSize)
                .ToListAsync();

            foreach (var item in itemsOnPage)
                _logger.LogInformation($"Data of item:" + JsonConvert.SerializeObject(item));

            return new PaginatedItems<PostItemEntity>() { TotalCount = totalItems, Data = itemsOnPage };
        }

        public async Task<List<PostLikeEntity>> GetPostLikesByUserIdAsync(string id)
            => await _dbContext.PostLikeEntity.Where(w => w.UserId == id).ToListAsync();
    }
}
