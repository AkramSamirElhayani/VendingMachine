using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;

namespace VendingMachine.Applicaion.Sellers.Command.AddProductAmount
{
    public class AddProductAmountCommandHandler : ICommandHandler<AddProductAmountCommand, int>
    {
         
        private readonly IProductServices _productServices;

        public AddProductAmountCommandHandler(IProductServices productServices)
        {
            _productServices = productServices;
        }

        public async Task<Result<int>> Handle(AddProductAmountCommand request, CancellationToken cancellationToken)
        {
            var result = await _productServices.AddProductToInventory(request.productId, request.Amount, cancellationToken);
            return result;
            
        }
    }
}
