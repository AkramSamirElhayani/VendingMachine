using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Interfaces;

namespace VendingMachine.Domain.Core;

public abstract class Entity : IEntity
{
    protected Entity(Guid id)
    {

        Id = id;
    }

    public Guid Id { get; private set; }
}