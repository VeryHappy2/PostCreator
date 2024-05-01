using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Post.Host.Data;
using Post.Host.Data.Entities;
using Post.Host.Repositories.Interfaces;

namespace Post.Host.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _dbset;
        private T _result;

        public Repository(IDbContextWrapper<ApplicationDbContext> context)
        {
            _dbContext = context.DbContext;
            _dbset = context.DbContext.Set<T>();
        }

        public async Task<int?> AddAsync(T entity)
        {
            var item = await _dbset.AddAsync(entity);

            if (item == null)
            {
                return null;
            }

            await _dbContext.SaveChangesAsync();
            return item.Entity.Id;
        }

        public async Task<int?> UpdateAsync(int id, T entity)
        {
			var item = await _dbset.FindAsync(id);

            if (item != null)
            {
                _dbContext.Entry(_result).CurrentValues.SetValues(entity);
                await _dbContext.SaveChangesAsync();
                return _result.Id;
            }

            return null;
        }

        public async Task<string?> DeleteAsync(int id)
        {
            _result = await _dbset
                .FindAsync(id);

            if (_result != null)
            {
                _dbset.Remove(_result);
                await _dbContext.SaveChangesAsync();
                return $"Object with id: {id} was successfully removed";
            }

            return null;
        }
    }
}
