using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Domain.Exeptions
{
    public class InvalidPriceExeption:Exception
    {
        public InvalidPriceExeption( int smallestCoin) : base($"Invalid Price \n the price must be dividable by ${smallestCoin}") { }
    }
}
