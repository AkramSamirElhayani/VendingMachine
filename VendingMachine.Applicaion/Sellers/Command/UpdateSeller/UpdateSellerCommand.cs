using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;

namespace VendingMachine.Applicaion.Sellers.Command.UpdateSeller;

public sealed record UpdateSellerCommand(Guid SellerId, string Name ) : ICommand
{
}
