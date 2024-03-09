using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;

namespace VendingMachine.Domain.Models;

public class InventoryTransaction:Entity
{


    private InventoryTransaction(Guid id, Guid productId, int count, DateTime date, InventoryTransactionType transactionType, int unitPrice) : base(id)
    {
        ProductId = productId;
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

    public Guid ProductId { get;private set; }
    public int Count { get; private set; }
    public int UnitPrice { get; private set; }
    public DateTime Date { get; private set; }
    public InventoryTransactionType TransactionType { get; set; }
}
public enum InventoryTransactionType
{
    Add,
    Remove
}