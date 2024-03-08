using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;
using VendingMachine.Infrastructer.Interfaces;

namespace VendingMachine.Infrastructer.Repository; internal class FinancialTransactionRepository : IFinancialTransactionRepository
{
    private readonly IVendingDbContext _dbContext;
    private readonly IVendingDapperContext _dapperContext;

    public FinancialTransactionRepository(IVendingDbContext dbContext, IVendingDapperContext dapperContext)
    {
        _dbContext = dbContext;
        _dapperContext = dapperContext;
    }

    public void Insert(FinancialTransaction entity) => _dbContext.Insert(entity);

    public async Task<int> GetBuyerBalanceAsync(Guid buyerId, CancellationToken ct)
    {
        var query = @"
            SELECT
                SUM(CASE WHEN TransactionType = @DepositType THEN Coin * CoinCount ELSE 0 END) -
                SUM(CASE WHEN TransactionType = @WithdrawType THEN Coin * CoinCount ELSE 0 END) AS Balance
            FROM FinancialTransactions
            WHERE BuyerId = @BuyerId";

        var parameters = new
        {
            BuyerId = buyerId,
            DepositType = (int)FinancialTransactionType.Deposit,
            WithdrawType = (int)FinancialTransactionType.Withdraw
        };

        var balance = await _dapperContext.ExecuteScalarAsync<int>(query, parameters, ct);

        return balance;
    }

    public async Task<Dictionary<int, int>> GetAvalibleCoinsAsync(CancellationToken ct)
    {
        var query = @"
        SELECT Coin, SUM(CoinCount) AS Count
        FROM FinancialTransactions
        WHERE TransactionType = @DepositType
        GROUP BY Coin";

        var parameters = new
        {
            DepositType = (int)FinancialTransactionType.Deposit
        };

        var availableCoins = await _dapperContext.QueryAsync<CoinCount>(query, parameters);
        
        

        return availableCoins.ToDictionary(kvp => kvp.Coin, kvp => kvp.Count);
    }
    private class CoinCount
    {
        public int Coin { get; set; }
        public int Count { get; set; }
    }
}