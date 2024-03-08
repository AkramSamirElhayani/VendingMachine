using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Interfaces
{

    public interface IProductServices
    {
        Task<Guid> CreateProductAsync(Product product, CancellationToken ct);
        Task<Result> UpdateProductInfoAsync(Guid id, string name, string? description, CancellationToken cancellationToken = default);
        Task<Result> UpdateProductPriceAsync(Guid id, int price, CancellationToken cancellationToken = default);
        Task<Result<int>> AddProductToInventory(Guid productId, int count, CancellationToken ct = default);
        Task<Result> Despense(Guid productId, int count, CancellationToken ct = default);

    }
}
