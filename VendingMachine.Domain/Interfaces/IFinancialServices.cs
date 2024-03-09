using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Exeptions;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Interfaces;

public interface IFinancialServices
{
    Task<Result> DepositAsync(Guid buyerId, Dictionary<int, int> curvals, CancellationToken ct);
    Task<Result<int>> GetBuyerBalanceAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Dictionary<int, int>>> WithdrawAllBalanceAsync(Guid buyerId, CancellationToken ct);
    Task<int> GetTotalSoldProductsPriceSumAsync(Guid id, CancellationToken cancellationToken);
    Task<Result> Credit(Guid buyerId,int amount, CancellationToken cancellationToken);
}
