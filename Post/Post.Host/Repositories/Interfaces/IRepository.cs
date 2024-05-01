using System.Threading.Tasks;

namespace Post.Host.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<int?> AddAsync(T entity);
        Task<int?> UpdateAsync(T entity);
        Task<string?> DeleteAsync(int id);
    }
}
