using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Exeptions;

namespace VendingMachine.Domain.Models;

public class Product:Entity
{


  
    private Product(Guid id, string name, Guid sellerId, string? description, int price ) : base(id)
    {
        Name = name;
        Description = description;
        SellerId = sellerId;
        Price = price;
    }


    public static Product Create(string name ,int price , Guid sellerId , string? description)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidNameExeption();
        if(!(price%FinancialTransaction.AllowedCoins.Min() == 0))
            throw new InvalidPriceExeption(FinancialTransaction.AllowedCoins.Min());

        return new Product(Guid.NewGuid(), name, sellerId, description,price);
    }

    public  Result Update(string name, string? description)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidNameExeption();

        Name = name;
        Description = description;
        return Result.Success();
    }
    public Result UpdatePrice(int price)
    {
        if (!(price % FinancialTransaction.AllowedCoins.Min() == 0))
            throw new InvalidPriceExeption(FinancialTransaction.AllowedCoins.Min());

        Price = price;
        return Result.Success();
    }

 

    public string Name { get;private set; }
    public int Price { get;private set; }
    public string? Description { get;private set; }
    public Guid SellerId { get; private set; }
     
}
