﻿using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Post.Host.Data;
using Post.Host.Data.Entities;
using Post.Host.Models.Responses;
using Post.Host.Repositories.Interfaces;

namespace Post.Host.Repositories
{
    public class PostItemRepository : IPostItemRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<PostItemEntity> _dbSet;

        public PostItemRepository(IDbContextWrapper<ApplicationDbContext> context)
        {
            _dbContext = context.DbContext;
            _dbSet = context.DbContext.Set<PostItemEntity>();
        }

        public async Task<GeneralResponse> DeleteByUserIdAsync(string userId)
        {
            var postItems = await _dbSet
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (postItems.Any())
            {
                _dbSet.RemoveRange(postItems);
                await _dbContext.SaveChangesAsync();

                return new GeneralResponse(true, "Post items were deleted");
            }

            return new GeneralResponse(false, "Post items weren't deleted");
        }

        public async Task<PostItemEntity> GetByIdAsync(int id)
            => await _dbSet
                .Include(x => x.Likes)
                .Include(x => x.Category)
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == id);
    }
}
