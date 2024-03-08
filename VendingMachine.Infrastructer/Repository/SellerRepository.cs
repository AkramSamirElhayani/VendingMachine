using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;
using VendingMachine.Infrastructer.Interfaces;

namespace VendingMachine.Infrastructer.Repository;
internal class SellerRepository : GenericRepository<Seller>, ISellerRepository
{
    public SellerRepository(IVendingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Seller?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(seller => seller.Name == name, cancellationToken);

    public async Task<bool> IsNameUniqueAsync(Guid excludedId, string name, CancellationToken cancellationToken = default)
        => !await AnyAsync(seller => seller.Id != excludedId && seller.Name == name, cancellationToken);
}
