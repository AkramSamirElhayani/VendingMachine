using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Domain.Core;

public abstract class Actor:Entity
{
 
    public string Name { get; protected set; }

    public Actor(string name, Guid id):base(id)
    {
        Name = name; 
    }
}
