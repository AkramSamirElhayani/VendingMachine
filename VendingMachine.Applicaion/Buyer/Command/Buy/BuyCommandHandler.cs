using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Exeptions;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services;

namespace VendingMachine.Applicaion.Buyers.Command.Buy;

public class BuyCommandHandler : ICommandHandler<BuyCommand, BuyCommandResponse>
{
  
    private readonly IProductServices _productServices;
    private readonly IFinancialServices _financialServices;
    private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public BuyCommandHandler(IProductServices productServices, IFinancialServices financialServices, IUnitOfWork unitOfWork, IProductRepository productRepository, IInventoryTransactionRepository inventoryTransactionRepository)
    {

        _productServices = productServices;
        _financialServices = financialServices;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _inventoryTransactionRepository = inventoryTransactionRepository;
    }

    public async Task<Result<BuyCommandResponse>> Handle(BuyCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository .GetByIdAsync(request.ProductId, cancellationToken); 
        if(product == null) 
            return Result.Failure<BuyCommandResponse>(Error.CreateFormExeption(new EntityNotFoundException(typeof(Product),request.ProductId)));

        Result<int> balanceResult = await _financialServices.GetBuyerBalanceAsync(request.BuyerId, cancellationToken);
        if (balanceResult.IsFailure)
            return Result.Failure<BuyCommandResponse>(balanceResult.Errors);

        if (balanceResult.Value < request.Amount * product.Price)
            return Result.Failure<BuyCommandResponse>(Error.CreateFormExeption(new InsufficantBalanceException()));

    
      
            Result result = await _productServices.Despense(request.ProductId, request.Amount, cancellationToken);
            
            if (result.IsFailure)
            {
                return Result.Failure<BuyCommandResponse>(result.Errors);
            }
            result = await _financialServices.Credit(request.BuyerId, request.Amount*product.Price, cancellationToken);
            Dictionary<int, int> change;
            if(balanceResult.Value > (product.Price * request.Amount))
            {
                var changeResult = await _financialServices.WithdrawAllBalanceAsync(request.BuyerId, cancellationToken);
                if (changeResult.IsFailure)
                {
                    return Result.Failure<BuyCommandResponse>(changeResult.Errors);
                }
                change = changeResult.Value;
            }else
                change = new Dictionary<int, int>();
            var buyerSpends = await _financialServices.GetTotalSoldProductsPriceSumAsync(request.BuyerId, cancellationToken);


            return new BuyCommandResponse(buyerSpends, product, change);
      
    }
}
