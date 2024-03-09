using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain;
using VendingMachine.Domain.Models;

namespace VendingMachine.Applicaion.Products.Queries.GetProductInfo
{
    public record GetProductInfoQuery(Guid productId):IQuery<ProductInfo>
    {
    }
}
