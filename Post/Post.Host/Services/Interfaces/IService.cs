using Post.Host.Data.Entities;
using Post.Host.Models.Dtos;

namespace Post.Host.Services.Interfaces
{
	public interface IService<TEntity, TDto>
		where TEntity : BaseEntity
		where TDto : BaseDto
	{
		Task<int?> AddAsync(TEntity entity);
		Task<int?> UpdateAsync(TEntity entity);
		Task<string?> DeleteAsync(int id);
		Task<TDto?> GetByIdAsync(int id);
	}
}
