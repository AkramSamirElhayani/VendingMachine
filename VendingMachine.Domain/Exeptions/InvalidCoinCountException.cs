using VendingMachine.Domain.Exeptions;

public class InvalidCoinCountException : DomainExeption
{
    public InvalidCoinCountException(int coin):base($"{coin} is Not a Valid Coin Count , It should be More than Zero ")
    {
        
    }
}
