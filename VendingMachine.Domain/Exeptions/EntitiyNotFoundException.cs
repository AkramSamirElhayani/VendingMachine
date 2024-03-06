using VendingMachine.Domain.Core;

public class EntitiyNotFoundException : System.Exception
{
    public EntitiyNotFoundException(Entity entity) : base($"{entity.GetType().Name} with Id {entity.Id} was not found")
    {
    }
    public EntitiyNotFoundException(Type entityType ,Guid id) : base($"{entityType.Name} with Id {id} was not found")
    {
    }
}
