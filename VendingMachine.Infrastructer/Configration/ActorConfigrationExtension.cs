using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;

namespace VendingMachine.Infrastructer.Configration;

internal static class ActorConfigrationExtension
{

    internal static void ConfiguerByActor<TActor>(this EntityTypeBuilder<TActor> builder)
        where TActor :Actor
    {

        builder.Property(x => x.Name).IsRequired().HasMaxLength(200); 

    }
}
