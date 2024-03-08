using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;

namespace VendingMachine.Applicaion.Buyers.Command.UpdateBuyer
{
    public sealed record UpdateBuyerCommand(Guid BuyerId ,string Name) : ICommand 
    {
    }
}
