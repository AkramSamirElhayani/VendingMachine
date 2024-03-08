using VendingMachine.Domain.Exeptions;

public class InvalidNameExeption : DomainExeption
{
    public InvalidNameExeption() : base("Invalid Name")
    {
    }
}
