using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Validation;

namespace VendingMachine.Domain.Models;

public class Buyer:Actor
{
    private Buyer(Guid id,string name) : base(name, id)
    {

    }
    public static Buyer Create(string name)
    {
        if(string.IsNullOrEmpty( name)) 
            throw new InvalidNameExeption();

        return new Buyer( Guid.NewGuid(),name);
    }
    public Result Update(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidNameExeption();

        Name = name; 
        return Result.Success();
    }
}
