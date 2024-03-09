using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;
using VendingMachine.Infrastructer.Interfaces;

namespace VendingMachine.Infrastructer.Repository;
// InventoryTransactionRepository
internal class InventoryTransactionRepository : IInventoryTransactionRepository
{
    private readonly IVendingDbContext _dbContext;

    public InventoryTransactionRepository(IVendingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Insert(InventoryTransaction entity) => _dbContext.Insert(entity);

    public async  Task<int> GetProductBalanceAsync(Guid productId, CancellationToken ct)
    {
        var depositsCount =   _dbContext.Set<InventoryTransaction>()
            .Where(it => it.ProductId == productId && it.TransactionType == InventoryTransactionType.Add)
            .Sum(it => it.Count);

        var withdrawalsCount =   _dbContext.Set<InventoryTransaction>()
            .Where(it => it.ProductId == productId && it.TransactionType == InventoryTransactionType.Remove)
            .Sum(it => it.Count);

        return (int)(depositsCount - withdrawalsCount);
    }

 
}