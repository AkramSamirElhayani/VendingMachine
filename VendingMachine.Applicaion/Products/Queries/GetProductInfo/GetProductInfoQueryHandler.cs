using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;

namespace VendingMachine.Applicaion.Products.Queries.GetProductInfo;

public class GetProductInfoQueryHandler : IQueryHandler<GetProductInfoQuery, ProductInfo>
{

    private readonly IProductRepository _productRepository;

    public GetProductInfoQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductInfo>> Handle(GetProductInfoQuery request, CancellationToken cancellationToken)
    {
        var product =await _productRepository.GetProductWithBalanceAsync(request.productId);
        if (product == null)
            return Result.Failure<ProductInfo>(Error.CreateFormExeption(new EntityNotFoundException(typeof(ProductInfo), request.productId)));
        return product;
    }
}
