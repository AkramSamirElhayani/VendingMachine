using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Domain.Interfaces;
public interface IEntity
{
    public Guid Id { get; }
}
