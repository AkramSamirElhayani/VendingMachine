using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;

namespace VendingMachine.Applicaion.Buyers.Command.CreateBuyer
{
    public sealed record CreateBuyerCommand(string Name):ICommand<Guid>
    {

    }
}
