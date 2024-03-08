using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;

namespace VendingMachine.Applicaion.Buyers.Command.Deposit
{
    public sealed class DepositCommandHandler : ICommandHandler<DepositCommand, int>
    {
        private readonly IFinancialServices financialServices;
        

        public DepositCommandHandler(IFinancialServices financialServices)
        {
            this.financialServices = financialServices;
        }

        public async Task<Result<int>> Handle(DepositCommand request, CancellationToken cancellationToken)
        {

            var result = await financialServices.DepositAsync(request.BuyerId, request.Coins, cancellationToken);
            if (result.IsFailure)
                return Result.Failure<int>(result.Errors);
            var balance =await financialServices.GetBuyerBalanceAsync(request.BuyerId,cancellationToken);

            return balance;

        }
    }
}
