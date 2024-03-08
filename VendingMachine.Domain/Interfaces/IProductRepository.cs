using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Interfaces;

public interface IProductRepository:IGenericRepository<Product>
{
    Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> IsNameUniqueAsync(Guid execludedId, string name, CancellationToken cancellationToken = default);
 

}
