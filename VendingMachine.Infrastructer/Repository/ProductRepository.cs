using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;
using VendingMachine.Infrastructer.Interfaces;

namespace VendingMachine.Infrastructer.Repository;
internal class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(IVendingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(product => product.Name == name, cancellationToken);

    public async Task<bool> IsNameUniqueAsync(Guid excludedId, string name, CancellationToken cancellationToken = default)
        => !await AnyAsync(product => product.Id != excludedId && product.Name == name, cancellationToken);

    public override void Remove(Product entity)
    {
        DbContext.Set<Product>().Entry(entity).Property<bool>("IsDeleted").CurrentValue = true;
        DbContext.Set<Product>().Entry(entity).State = EntityState.Modified;
    }
}