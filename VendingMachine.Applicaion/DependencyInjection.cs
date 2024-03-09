using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Services;

namespace VendingMachine.Applicaion;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(RequstLoggingPiplineBehavior<,>));
        });
        services.AddScoped<SellerServices>();
        services.AddScoped<IBuyerServices,BuyerServices>();
        services.AddScoped<IFinancialServices,FinancialServices>();
        services.AddScoped<IProductServices, ProductServices>();
        
        return services;
    }
}
