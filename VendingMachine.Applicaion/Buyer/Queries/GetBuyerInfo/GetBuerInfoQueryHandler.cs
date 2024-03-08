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

namespace VendingMachine.Applicaion.Buyers.Queries.GetBuyerInfo
{
    public sealed class GetBuerInfoQueryHandler : IQueryHandler<GetBuyerInfoQuery, Buyer>
    {
        private readonly IBuyerRepository _buyerRepository;

        public GetBuerInfoQueryHandler(IBuyerRepository buyerRepository)
        {
            _buyerRepository = buyerRepository;
        }

        public   async Task<Result<Buyer>>  Handle(GetBuyerInfoQuery request, CancellationToken cancellationToken)
        {
            var buyer =await _buyerRepository.GetByIdAsync(request.id, cancellationToken);
            if( buyer == null ) 
                return Result.Failure<Buyer>(Error.CreateFormExeption(new EntityNotFoundException(typeof(Buyer),request.id)));    

            return buyer;
        }
    }
}
