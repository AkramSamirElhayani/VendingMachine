namespace VendingMachine.Contract;

public class ProductResponse
{

    public Guid Id { get; set; }
    public string Name { get;   set; }
    public int Price { get;   set; }
    public string? Description { get;   set; }
    public Guid SellerId { get;   set; }
     
}
