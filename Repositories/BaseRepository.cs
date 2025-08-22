using Microsoft.EntityFrameworkCore;
using UsersTeachers.Data;
using UsersTeachers.Repositories;

namespace UsersTeachers.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{

    protected readonly UsersTeachersDbContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(UsersTeachersDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        T? existing = await _dbSet.FindAsync(id);
        if (existing is not null)
        {
            _dbSet.Remove(existing);
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var entities = await _dbSet.ToListAsync();
        return entities;
    }

    public async Task<T> GetAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity!;
    }

    public async Task<int> GetCountAsync()
    {
        var count = await _dbSet.CountAsync();
        return count;
    }

    public void UpdateAsync(T entity)
    {
        //_dbSet.Attach(entity);
        //_context.Entry(entity).State = EntityState.Modified;
        _dbSet.Update(entity);
    }
}

