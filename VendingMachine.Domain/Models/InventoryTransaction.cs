using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;

namespace VendingMachine.Domain.Models;

public class InventoryTransaction:Entity
{


    private InventoryTransaction(Guid id, Guid porductId, int count, DateTime date, InventoryTransactionType transactionType, int unitPrice) : base(id)
    {
        PorductId = porductId;
        Count = count;
        Date = date;
        TransactionType = transactionType;
        UnitPrice = unitPrice;
    }
    public static InventoryTransaction Create(Guid productId , InventoryTransactionType transactionType, int count,int unitPrice)
    {
        if (count <= 0)
            throw new ArgumentException("Count Must be more than zero", nameof(count));

        if (unitPrice <= 0)
            throw new ArgumentException("UnitPrice Must be more than zero", nameof(count));


        return new InventoryTransaction(Guid.NewGuid(), productId, count, DateTime.Now,transactionType, unitPrice);
    }

    public Guid PorductId { get; set; }
    public int Count { get; set; }
    public int UnitPrice { get; set; }
    public DateTime Date { get; set; }
    public InventoryTransactionType TransactionType { get; set; }
}
public enum InventoryTransactionType
{
    Add,
    Remove
}