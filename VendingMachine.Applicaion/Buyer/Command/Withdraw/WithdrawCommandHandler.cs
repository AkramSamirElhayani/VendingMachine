using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;

namespace VendingMachine.Applicaion.Buyers.Command.Withdraw
{
    public class WithdrawCommandHandler : ICommandHandler<WithdrawCommand, Dictionary<int, int>>
    {
        private readonly IFinancialServices financialServices;

        public WithdrawCommandHandler(IFinancialServices financialServices)
        {
            this.financialServices = financialServices;
        }

        public Task<Result<Dictionary<int, int>>> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
          return  financialServices.WithdrawAllBalanceAsync(request.BuyerId, cancellationToken);
        }
    }
}
