using System.Collections.Immutable;
using VendingMachine.Domain.Core;

namespace VendingMachine.Domain.Models;

public class FinancialTransaction : Entity
{
    public static readonly ImmutableArray<int> AllowedCoins = [5, 10, 20, 50, 100];

    private FinancialTransaction(Guid id, Guid buyerId, FinancialTransactionType transactionType, int coin, int coinCount, DateTime date) : base(id)
    {
        BuyerId = buyerId;
        TransactionType = transactionType;
        Coin = coin;
        CoinCount = coinCount;
        Date = date;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buyerId"></param>
    /// <param name="transactionType"></param>
    /// <param name="coin"></param>
    /// <param name="coinCount"></param>
    /// <returns></returns>
    /// <exception cref="InvalidCoinCountException"></exception>
    /// <exception cref="InvalidCoinException"></exception>
    public static FinancialTransaction Create(Guid buyerId, FinancialTransactionType transactionType, int coin, int coinCount)
    {
        if (coinCount <= 0)
            throw new InvalidCoinCountException(coin);
        if (!AllowedCoins.Contains(coin))
            throw new InvalidCoinException(coin);
        return new FinancialTransaction(Guid.NewGuid(), buyerId, transactionType, coin, coinCount,DateTime.Now);
    }
    public Guid BuyerId { get; set; }
    public FinancialTransactionType TransactionType { get; set; }
    //This Should Be of type a decimal or a double in most cases ,
    //but since the Vending Machine will be accepting only 5,10,20,50 and 100 coins ,
    //we will keep it int 
    public int Coin { get; set; }
    public int CoinCount { get; set; }
    public DateTime Date { get; set; }



}
public enum FinancialTransactionType
{
    Deposit,
    Withdraw,
    Credited
}
