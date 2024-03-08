using VendingMachine.Domain.Core;
using VendingMachine.Domain.Exeptions;

public class EntityNotFoundException : DomainExeption
{
    public EntityNotFoundException(Entity entity) : base($"{entity.GetType().Name} with Id {entity.Id} was not found")
    {
    }
    public EntityNotFoundException(Type entityType ,Guid id) : base($"{entityType.Name} with Id {id} was not found")
    {
    }
}
