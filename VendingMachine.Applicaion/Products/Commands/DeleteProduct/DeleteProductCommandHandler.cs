using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Applicaion.Products.Commands.UpdateProduct;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;

namespace VendingMachine.Applicaion.Products.Commands.DeleteProduct;

internal class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductServices _productServices;

    public DeleteProductCommandHandler(IProductServices productServices)
    {
        _productServices = productServices;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {


        var result = await _productServices.DeleteProductAsync(request.ProductId ,cancellationToken);

        return result;

    }
}