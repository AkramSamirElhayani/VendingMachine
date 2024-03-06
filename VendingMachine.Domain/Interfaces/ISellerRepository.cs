using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Domain.Interfaces;

internal interface ISellerRepositoryIGenericRepository<Seller>
{
    Task<Seller?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> IsNameUniqueAsync(Guid execludedId, string name, CancellationToken cancellationToken = default);

}
