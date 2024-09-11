namespace Post.Host.Services.Interfaces
{
	public interface IService<T>
	{
		Task<int?> AddAsync(T entity);
		Task<int?> UpdateAsync(T entity);
		Task<string?> DeleteAsync(int id);
		Task<T?> GetByIdAsync(int id);
	}
}
