using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Interfaces;

public interface IInventoryTransactionRepository
{


    void Insert(InventoryTransaction entity);
    //void Remove(InventoryTransaction entity);
    Task<int> GetProductBalanceAsync(Guid productId ,CancellationToken ct);
 

}
