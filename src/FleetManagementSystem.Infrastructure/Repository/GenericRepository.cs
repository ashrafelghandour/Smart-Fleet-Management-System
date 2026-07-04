using Microsoft.EntityFrameworkCore;
using FleetManagementSystem.Domain.Common;
using System.Linq.Expressions;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Infrastructure.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FleetManagementSystem.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }
    public async Task<T>Update(T entity)
    {
        _dbSet.Update(entity);
        return   entity;
    }

    public  void Delete(T entity)
    {
        

        entity.IsDeleted = true;  
          Update(entity);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

}