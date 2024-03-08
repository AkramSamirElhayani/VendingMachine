using VendingMachine.Domain.Exeptions;
using VendingMachine.Domain.Models;

public class InvalidCoinException : DomainExeption
{
    public InvalidCoinException(int coin):base($"{coin} Is Not a valid Coin \n it should be one of the following [{string.Join(",",FinancialTransaction.AllowedCoins)}]")
    {
    }
}
