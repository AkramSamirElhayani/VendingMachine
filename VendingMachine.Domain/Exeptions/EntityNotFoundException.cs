using VendingMachine.Domain.Core;

public class EntityNotFoundException : System.Exception
{
    public EntityNotFoundException(Entity entity) : base($"{entity.GetType().Name} with Id {entity.Id} was not found")
    {
    }
    public EntityNotFoundException(Type entityType ,Guid id) : base($"{entityType.Name} with Id {id} was not found")
    {
    }
}
