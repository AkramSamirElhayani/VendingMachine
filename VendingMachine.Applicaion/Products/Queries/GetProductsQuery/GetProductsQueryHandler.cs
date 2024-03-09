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

namespace VendingMachine.Applicaion.Products.Queries.GetProductsQuery;

public class GetProductsQueryHandler:IQueryHandler<GetProductsQuery, List<ProductInfo>>
{

    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<List<ProductInfo>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductsWithBalanceAsync();
        if (product == null)
            product = Enumerable.Empty<ProductInfo>().ToList();
        return product;
    }
}
