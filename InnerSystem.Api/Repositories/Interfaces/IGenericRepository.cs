using System.Linq.Expressions;

namespace InnerSystem.Api.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
	Task<T?> GetByIdAsync(Guid id);
	Task<T?> GetByIdAsync(
	Guid id,
	params Expression<Func<T, object>>[] includes);
	Task<IEnumerable<T>> GetAllAsync();
	Task AddAsync(T entity);
	void Update(T entity);
	void Delete(T entity);
	Task<bool> SaveChangesAsync();
}
