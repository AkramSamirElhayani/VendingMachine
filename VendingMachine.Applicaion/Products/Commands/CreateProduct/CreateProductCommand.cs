using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain.Models;

namespace VendingMachine.Applicaion.Products.Commands.CreateProduct;

public record CreateProductCommand(string Name, int Price, Guid SellerId, string? Description) :ICommand<Product>
{
}
