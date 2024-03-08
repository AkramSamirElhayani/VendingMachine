using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Infrastructer.Interfaces;

namespace VendingMachine.Infrastructer.Repository;

public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : Entity
{
    protected readonly IVendingDbContext DbContext;

    protected GenericRepository(IVendingDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void Insert(TEntity entity) => DbContext.Insert(entity);

    public void Update(TEntity entity) => DbContext.Set<TEntity>().Update(entity);

    public void Remove(TEntity entity) => DbContext.Remove(entity);

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await DbContext.GetByIdAsync<TEntity>(id, cancellationToken);

    public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => await DbContext.Set<TEntity>().ToListAsync(cancellationToken);

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        => DbContext.Set<TEntity>().Where(expression);

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        => await DbContext.Set<TEntity>().AnyAsync(expression, cancellationToken);

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        => await DbContext.Set<TEntity>().FirstOrDefaultAsync(expression, cancellationToken);
}
