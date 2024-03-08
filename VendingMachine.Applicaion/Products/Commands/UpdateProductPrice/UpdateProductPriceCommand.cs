using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;

namespace VendingMachine.Applicaion.Products.Commands.UpdateProductPrice;

public sealed record UpdateProductPriceCommand(Guid ProductId ,int Price) :ICommand;

