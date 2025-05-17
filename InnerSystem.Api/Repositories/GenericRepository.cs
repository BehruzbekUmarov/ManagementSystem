using InnerSystem.Api.Data;
using InnerSystem.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnerSystem.Api.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
	private readonly AppDbContext _context;
	private readonly DbSet<T> _dbSet;

	public GenericRepository(AppDbContext context)
	{
		_context = context;
		_dbSet = context.Set<T>();
	}

	public async Task<T?> GetByIdAsync(Guid id)
	{
		return await _dbSet.FindAsync(id);
	}

	public async Task<T?> GetByIdAsync(
	Guid id,
	params Expression<Func<T, object>>[] includes)
	{
		IQueryable<T> query = _dbSet;

		foreach (var include in includes)
		{
			query = query.Include(include);
		}

		return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
	}

	public async Task<IEnumerable<T>> GetAllAsync()
	{
		return await _dbSet.ToListAsync();
	}

	public async Task AddAsync(T entity)
	{
		await _dbSet.AddAsync(entity);
	}

	public void Update(T entity)
	{
		_dbSet.Update(entity);
	}

	public void Delete(T entity)
	{
		_dbSet.Remove(entity);
	}

	public async Task<bool> SaveChangesAsync()
	{
		return await _context.SaveChangesAsync() > 0;
	}
}