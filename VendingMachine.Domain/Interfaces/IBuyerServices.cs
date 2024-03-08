using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Interfaces;

public interface IBuyerServices
{

   Task<Guid> CreateBuyerAsync(Buyer buyer, CancellationToken ct);
   Task<Result> UpdateBuyerAsync(Guid id, string name, CancellationToken cancellationToken = default);

}
