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
        var parameters = new
        {
            BuyerId = buyerId,
            DepositType = (int)FinancialTransactionType.Deposit,
            WithdrawType = (int)FinancialTransactionType.Withdraw,
            CreditedType = (int)FinancialTransactionType.Credited,
        };

        var totalDepositedQuery = @"
            SELECT SUM(CoinCount * Coin) AS TotalDeposited
            FROM FinancialTransaction
            WHERE BuyerId = @BuyerId AND TransactionType = @DepositType";

        var totalWithdrawenQuery = @"
            SELECT SUM(CoinCount * Coin) AS TotalDeposited
            FROM FinancialTransaction
            WHERE BuyerId = @BuyerId AND TransactionType = @WithdrawType";

        var totalCreditedQuery = @"
            SELECT SUM(CoinCount * Coin) AS TotalDeposited
            FROM FinancialTransaction
            WHERE BuyerId = @BuyerId AND TransactionType = @CreditedType";

        var deposits = await _dapperContext.ExecuteScalarAsync<int>(totalDepositedQuery, parameters, ct);
        var withdraws = await _dapperContext.ExecuteScalarAsync<int>(totalWithdrawenQuery, parameters, ct);
        var credits = await _dapperContext.ExecuteScalarAsync<int>(totalCreditedQuery, parameters, ct);

        return deposits - withdraws - credits;
    }

    public async Task<Dictionary<int, int>> GetAvalibleCoinsAsync(CancellationToken ct)
    {
        var query = @"
        SELECT Coin, SUM(
CASE 
WHEN TransactionType = @DepositType  THEN CoinCount
WHEN TransactionType = @CreditedType     THEN 0 
ELSE -CoinCount 
END) AS Count
        FROM FinancialTransaction
        GROUP BY Coin";

        var parameters = new
        {
            DepositType = (int)FinancialTransactionType.Deposit,
            CreditedType = (int)FinancialTransactionType.Credited
        };


        var availableCoins = await _dapperContext.QueryAsync<CoinCount>(query, parameters);
        
        return availableCoins.ToDictionary(kvp => kvp.Coin, kvp => kvp.Count);
    }
    public async  Task<int> GetTotalSoldProductsPriceSumAsync(Guid buyerId,  CancellationToken cancellationToken)
    {
        var parameters = new
        {
            BuyerId = buyerId,
            DepositType = (int)FinancialTransactionType.Deposit,
            WithdrawType = (int)FinancialTransactionType.Withdraw,
            CreditedType = (int)FinancialTransactionType.Credited,
        };

        var totalCreditedQuery = @"
            SELECT SUM(CoinCount*Coin) AS TotalDeposited
            FROM FinancialTransaction
            WHERE BuyerId = @BuyerId AND TransactionType = @CreditedType";

        var credits = await _dapperContext.ExecuteScalarAsync<int>(totalCreditedQuery, parameters, cancellationToken);

        return credits;
    }
    private class CoinCount
    {
        public int Coin { get; set; }
        public int Count { get; set; }
    }
}