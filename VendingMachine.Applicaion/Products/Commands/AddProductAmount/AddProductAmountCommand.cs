using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;

namespace VendingMachine.Applicaion.Sellers.Command.AddProductAmount
{
    public  sealed record AddProductAmountCommand(Guid productId , int Amount):ICommand<int>
    {
    }
}
