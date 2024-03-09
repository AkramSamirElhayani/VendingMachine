using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Domain;

public class ProductInfo
{
    public Guid Id { get; set; }
    public Guid SellerId { get;   set; }
    public string Name { get;   set; }
    public int Price { get;   set; }
    public string? Description { get;   set; }
    public int AmountAvailable { get; set; }
    
        

}
