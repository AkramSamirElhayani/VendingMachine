using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain.Models;

namespace VendingMachine.Applicaion.Products.Queries.GetProductsQuery
{
    public sealed record GetProductsQuery: IQuery<List<Product>>
    {
    }
}
