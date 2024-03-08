using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Applicaion.Products.Commands.UpdateProduct;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;

namespace VendingMachine.Applicaion.Products.Commands.UpdateProductPrice;

internal class UpdateProductPriceCommandHandler : ICommandHandler<UpdateProductPriceCommand>
{
    private readonly IProductServices _productServices;

    public UpdateProductPriceCommandHandler(IProductServices productServices)
    {
        _productServices = productServices;
    }

    public async Task<Result> Handle(UpdateProductPriceCommand request, CancellationToken cancellationToken)
    {


        var result = await _productServices.UpdateProductPriceAsync(request.ProductId,  request.Price, cancellationToken);

        return result;

    }
}