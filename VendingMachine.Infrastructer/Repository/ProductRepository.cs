using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;
using VendingMachine.Infrastructer.Interfaces;

namespace VendingMachine.Infrastructer.Repository;
internal class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly IVendingDapperContext _dapperContext;
    public ProductRepository(IVendingDbContext dbContext, IVendingDapperContext dapperContext) : base(dbContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(product => product.Name == name, cancellationToken);

    public async Task<List<ProductInfo>> GetProductsWithBalanceAsync(CancellationToken cancellationToken = default)
    {
        var query = @"
        SELECT p.Id,p.SellerId, p.Name, p.Description, p.Price,
               COALESCE(SUM(CASE WHEN it.TransactionType = @DepositType THEN it.Count ELSE -it.Count END), 0) AS AmountAvailable
        FROM Product p
        LEFT JOIN InventoryTransaction it ON p.Id = it.ProductId
        WHERE p.IsDeleted = 0
        GROUP BY p.Id, p.Name, p.Description, p.Price,p.SellerId
    ";

        var parameters = new
        {
            DepositType = (int)InventoryTransactionType.Add
        };

    
        var productInfoList = await _dapperContext.QueryAsync<ProductInfo>(query, parameters, cancellationToken);

        return productInfoList.ToList();
    }

    public async Task<ProductInfo?> GetProductWithBalanceAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var query = @"
        SELECT p.Id,p.SellerId, p.Name, p.Description, p.Price,
               COALESCE(SUM(CASE WHEN it.TransactionType = @DepositType THEN it.Count ELSE -it.Count END), 0) AS AmountAvailable
        FROM Product p
        LEFT JOIN InventoryTransaction it ON p.Id = it.ProductId AND it.ProductId = @ProductId
        WHERE p.Id = @ProductId AND p.IsDeleted = 0
        GROUP BY p.Id, p.Name, p.Description, p.Price,p.SellerId
    ";

        var parameters = new
        {
            DepositType = (int)InventoryTransactionType.Add,
            ProductId = productId
        };

   
        var productInfo = await _dapperContext.QueryAsync<ProductInfo>(query, parameters, cancellationToken);

        return productInfo.FirstOrDefault();
    }

    public async Task<bool> IsNameUniqueAsync(Guid excludedId, string name, CancellationToken cancellationToken = default)
        => !await AnyAsync(product => product.Id != excludedId && product.Name == name, cancellationToken);

    public override void Remove(Product entity)
    {
        DbContext.Set<Product>().Entry(entity).Property<bool>("IsDeleted").CurrentValue = true;
        DbContext.Set<Product>().Entry(entity).State = EntityState.Modified;
    }
}