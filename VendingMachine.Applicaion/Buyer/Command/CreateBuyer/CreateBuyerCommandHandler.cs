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
using VendingMachine.Domain.Services;

namespace VendingMachine.Applicaion.Buyers.Command.CreateBuyer
{
    public class CreateBuyerCommandHandler : ICommandHandler<CreateBuyerCommand, Guid>
    {
        private readonly IBuyerServices _buyerServices;

        public CreateBuyerCommandHandler(IBuyerServices buyerServices)
        {
            _buyerServices = buyerServices;
        }

        public async Task<Result<Guid>> Handle(CreateBuyerCommand request, CancellationToken ct)
        {
            var buyer = Buyer.Create(request.Name);
            var result = await _buyerServices.CreateBuyerAsync(buyer, ct);
            return Result.Success(result);
        }
    }
}
