using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Models;

namespace VendingMachine.Infrastructer.Configration
{
    internal class InventoryTransactionConfigration : IEntityTypeConfiguration<InventoryTransaction>
    {
   public     void  Configure(EntityTypeBuilder<InventoryTransaction> builder)
        {
            builder.UseTpcMappingStrategy();

            builder
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(t => t.PorductId);
        }
    }
}
