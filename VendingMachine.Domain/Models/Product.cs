using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;

namespace VendingMachine.Domain.Models;

public class Product:Entity
{


    private Product(Guid id,string name,  Guid sellerId, string? description) : base(id)
    {
        Name = name;
        Description = description;
        SellerId = sellerId;
    }

    public static Product Create(string name , Guid sellerId , string? description)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidNameExeption();
        return new Product(Guid.NewGuid(), name, sellerId, description);
    }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid SellerId { get; set; }
}
