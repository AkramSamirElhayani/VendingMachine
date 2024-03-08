using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Infrastructer.Class;
using VendingMachine.Infrastructer.Context;
using VendingMachine.Infrastructer.Interfaces;
using VendingMachine.Infrastructer.Repository;

namespace VendingMachine.Infrastructer;
public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {


        string connectionString = configuration.GetConnectionString(VendingConnectionString.SettingsKey)!;

        services.AddSingleton(new VendingConnectionString(connectionString));

        services.AddDbContext<VendingDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IVendingDbContext>(serviceProvider => serviceProvider.GetRequiredService<VendingDbContext>());
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<VendingDbContext>());

        services.AddScoped<ISellerRepository, SellerRepository>();
        services.AddScoped<IBuyerRepository, BuyerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IFinancialTransactionRepository, FinancialTransactionRepository>();
        services.AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>();


        services.AddScoped<IVendingDapperContext, VendingDapperContext>();

        return services;
    }
}
