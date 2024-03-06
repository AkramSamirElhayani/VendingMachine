using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Interfaces;

internal interface IFinancialTransactionRepository:IGenericRepository<FinancialTransaction>
{
}
