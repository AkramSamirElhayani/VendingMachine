using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Domain.Exeptions;

public class InsufficantBalanceException:Exception
{
    public InsufficantBalanceException() : base("Balance is Insufficant")
    {

    }

}
