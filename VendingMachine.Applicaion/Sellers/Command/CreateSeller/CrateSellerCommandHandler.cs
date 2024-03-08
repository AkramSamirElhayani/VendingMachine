using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services;

namespace VendingMachine.Applicaion.Sellers.Command.CreateSeller;
public class CreateSellerCommandHandler : ICommandHandler<CreateSellerCommand, Guid>
{
    private readonly SellerServices _sellerServices;

    public CreateSellerCommandHandler(SellerServices sellerServices)
    {
        _sellerServices = sellerServices;
    }

    public async Task<Result<Guid>> Handle(CreateSellerCommand request, CancellationToken ct)
    {
        var seller = Seller.Create(request.Name);
        var result = await _sellerServices.CreateSellerAsync(seller, ct);
        return Result.Success(result);
    }
}
