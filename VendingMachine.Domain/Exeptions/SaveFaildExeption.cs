using VendingMachine.Domain.Exeptions;

public class SaveFaildExeption : DomainExeption
{
    public SaveFaildExeption(string str) : base(str)
    {
    }
}
