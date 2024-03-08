using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Applicaion.Products.Commands.CreateProduct;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;

namespace VendingMachine.Applicaion.Products.Commands.UpdateProduct;

internal class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductServices _productServices;

    public UpdateProductCommandHandler(IProductServices productServices)
    {
        _productServices = productServices;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {

       
      var result = await _productServices.UpdateProductInfoAsync(request.ProductId, request.Name,request.Description, cancellationToken);

       return result;

    }
}