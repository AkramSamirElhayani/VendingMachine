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
    internal class FinancialTransactionConfigration : IEntityTypeConfiguration<FinancialTransaction>
    {
        public void  Configure(EntityTypeBuilder<FinancialTransaction> builder)
        {
            builder.UseTpcMappingStrategy();

            builder
                .HasOne<Buyer>()
                .WithMany()
                .HasForeignKey(b => b.BuyerId);
        }
    }
}
