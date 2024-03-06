using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;

namespace VendingMachine.Domain.Models;

public class Seller : Actor
{
    private Seller(string name , Guid id) : base(name, id)
    {

    }

    public static Seller Create(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidNameExeption();

        return new Seller(name, Guid.NewGuid());
    }

    public Result Update(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidNameExeption();

        Name = name;
        return Result.Success();
    }
}
