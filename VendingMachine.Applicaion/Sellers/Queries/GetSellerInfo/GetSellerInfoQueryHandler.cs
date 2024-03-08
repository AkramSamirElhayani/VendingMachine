using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;

namespace VendingMachine.Applicaion.Sellers.Queries.GetSellerInfo;

public sealed class GetSellerInfoQueryHandler : IQueryHandler<GetSellerInfoQuery, Seller>
{
    private readonly ISellerRepository _sellerRepository;

    public GetSellerInfoQueryHandler(ISellerRepository sellerRepository)
    {
        _sellerRepository = sellerRepository;
    }

    public async Task<Result<Seller>> Handle(GetSellerInfoQuery request, CancellationToken cancellationToken)
    {
        var seller = await _sellerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (seller == null)
            return Result.Failure<Seller>(Error.CreateFormExeption(new EntityNotFoundException(typeof(Seller), request.Id)));

        return seller;
    }
}

