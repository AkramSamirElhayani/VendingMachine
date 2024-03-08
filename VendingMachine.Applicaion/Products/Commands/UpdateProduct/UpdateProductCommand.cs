using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;

namespace VendingMachine.Applicaion.Products.Commands.UpdateProduct
{
    public sealed record UpdateProductCommand(Guid ProductId ,string Name,  string? Description) :ICommand
    {
    }
}
