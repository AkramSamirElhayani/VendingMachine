using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;
using VendingMachine.Infrastructer.Interfaces;
namespace VendingMachine.Infrastructer.Repository;

internal class BuyerRepository : GenericRepository<Buyer>, IBuyerRepository
{
    public BuyerRepository(IVendingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Buyer?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(buyer => buyer.Name == name, cancellationToken);

    public async Task<bool> IsNameUniqueAsync(Guid excludedId, string name, CancellationToken cancellationToken = default)
        => !await AnyAsync(buyer => buyer.Id != excludedId && buyer.Name == name, cancellationToken);
}