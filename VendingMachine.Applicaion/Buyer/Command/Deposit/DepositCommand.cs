using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;

namespace VendingMachine.Applicaion.Buyers.Command.Deposit
{
    public sealed record DepositCommand(Guid BuyerId , Dictionary<int,int> Coins):ICommand<int>
    {
    }
}
