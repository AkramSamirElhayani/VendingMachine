using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;

namespace VendingMachine.Applicaion.Buyers.Command.UpdateBuyer
{
    internal class UpdateBuyerCommandHandler : ICommandHandler<UpdateBuyerCommand>
    {
        private readonly IBuyerServices _buyerServices;
        public async Task<Result> Handle(UpdateBuyerCommand request, CancellationToken cancellationToken)
        {
          return await _buyerServices.UpdateBuyerAsync(request.BuyerId, request.Name, cancellationToken);
        }
    }
}
