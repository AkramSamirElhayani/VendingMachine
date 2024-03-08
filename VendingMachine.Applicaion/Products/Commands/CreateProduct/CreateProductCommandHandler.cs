using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;

namespace VendingMachine.Applicaion.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
{
    private readonly IProductServices _productServices; 

    public CreateProductCommandHandler(IProductServices productServices)
    {
        _productServices = productServices;
    }

    public async Task<Result<Product>>  Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
       
        var product = Product.Create(request.Name, request.Price, request.SellerId, request.Description);
        await _productServices.CreateProductAsync(product, cancellationToken);

        return product;

    }
}
