﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Domain.Exeptions
{
    internal class InsufficantProductBalanceException: DomainExeption
    {
        public InsufficantProductBalanceException() : base("Product Balance is Insufficant")
        {
        }
    }
}
