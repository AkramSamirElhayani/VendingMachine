public class InvalidCoinCountException : System.Exception
{
    public InvalidCoinCountException(int coin):base($"{coin} is Not a Valid Coin Count , It should be More than Zero ")
    {
        
    }
}
