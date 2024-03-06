using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;

namespace VendingMachine.Domain.Models;

public class InventoryTransaction:Entity
{


    private InventoryTransaction(Guid id , Guid porductId, int count, DateTime date):base(id)
    {
        PorductId = porductId;
        Count = count;
        Date = date;
    }
    public static InventoryTransaction Create(Guid productId ,int count)
    {
        if (count >= 0)
            throw new ArgumentException("Count Must be more than zero", nameof(count));

        return new InventoryTransaction(Guid.NewGuid(), productId, count, DateTime.Now);
    }

    public Guid PorductId { get; set; }
    public int Count { get; set; }
    public DateTime Date { get; set; }
}
