using VendingMachine.Domain.Models;

public class InvalidCoinException : System.Exception
{
    public InvalidCoinException(int coin):base($"{coin} Is Not a valid Coin \n it should be one of the following [{string.Join(",",FinancialTransaction.AllowedCoins)}]")
    {
    }
}
