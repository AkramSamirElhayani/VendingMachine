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

    public async Task<int> GetProductBalanceAsync(Guid productId, CancellationToken ct)
    {
        // Implement the logic to get the product balance
        throw new NotImplementedException();
    }

    public async Task<int> GetTotalSoldProductsSumPriceAsync(Guid id, CancellationToken cancellationToken)
    {
        // Implement the logic to get the total sum of sold product prices
        throw new NotImplementedException();
    }
}