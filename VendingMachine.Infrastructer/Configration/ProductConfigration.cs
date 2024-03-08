using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Models;

namespace VendingMachine.Infrastructer.Configration
{
    internal class ProductConfigration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.UseTpcMappingStrategy();
            builder.Property<bool>("IsDeleted"); 
            builder.HasQueryFilter(e => !EF.Property<bool>(e, "IsDeleted"));
        }
    }
}
