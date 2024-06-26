﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Interfaces;

public interface IFinancialTransactionRepository
{


    void Insert(FinancialTransaction entity);
    Task<int> GetBuyerBalanceAsync(Guid buyerId , CancellationToken ct);
    Task<Dictionary<int,int>> GetAvalibleCoinsAsync(CancellationToken ct);
    Task<int> GetTotalSoldProductsPriceSumAsync(Guid id, CancellationToken cancellationToken);
}
