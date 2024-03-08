using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Services;

namespace VendingMachine.Applicaion.Sellers.Command.UpdateSeller;


public class UpdateSellerCommandHandler : ICommandHandler<UpdateSellerCommand>
{
    private readonly SellerServices _sellerServices;

    public UpdateSellerCommandHandler(SellerServices sellerServices)
    {
        _sellerServices = sellerServices;
    }

    public async Task<Result> Handle(UpdateSellerCommand request, CancellationToken cancellationToken)
    {
        return await _sellerServices.UpdateSellerAsync(request.SellerId, request.Name, cancellationToken);
    }
}