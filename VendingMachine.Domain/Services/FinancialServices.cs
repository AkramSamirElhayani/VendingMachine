using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Exeptions;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Services;

public class FinancialServices: IFinancialServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFinancialTransactionRepository _financialTransactionRepository;
    private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
    private readonly IBuyerRepository _buyerRepository;

    public FinancialServices(IFinancialTransactionRepository financialTransactionRepository, IUnitOfWork unitOfWork, IBuyerRepository buyerRepository, IInventoryTransactionRepository inventoryTransactionRepository)
    {
        _financialTransactionRepository = financialTransactionRepository;
        _unitOfWork = unitOfWork;
        _buyerRepository = buyerRepository;
        _inventoryTransactionRepository = inventoryTransactionRepository;
    }

    public async Task<Result> DepositAsync(Guid buyerId ,Dictionary<int,int> curvals ,CancellationToken ct)
    {
        if (!await _buyerRepository.AnyAsync(b => b.Id == buyerId,ct))
            return Result.Failure(Error.CreateFormExeption(new EntityNotFoundException(typeof(Buyer), buyerId)));


        foreach (var item in curvals)
        {
            _financialTransactionRepository.Insert(FinancialTransaction.Create(buyerId,FinancialTransactionType.Deposit,item.Key,item.Value));        
        }

        var result = await _unitOfWork.SaveChangesAsync(ct);
        if (result == 0)
            throw new SaveFaildExeption("Something went wrong , buyer data was not saved");

        return Result.Success();
    }

    public async Task<Result<int>> GetBuyerBalanceAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existingBuyer = await _buyerRepository.GetByIdAsync(id);

        if (existingBuyer == null)
            return Result.Failure<int>(Error.CreateFormExeption(new EntityNotFoundException(typeof(Buyer), id)));
        
        var buyerDeposits =await _financialTransactionRepository.GetBuyerBalanceAsync(id, cancellationToken);
        var buyerSpends  =await _financialTransactionRepository.GetTotalSoldProductsPriceSumAsync(id , cancellationToken);
        return buyerDeposits - buyerSpends;


    }

    public async Task<int> GetTotalSoldProductsPriceSumAsync(Guid id, CancellationToken cancellationToken)
    {
      return  await _financialTransactionRepository.GetTotalSoldProductsPriceSumAsync(id, cancellationToken);
    }


    /// <summary>
    /// withdraw all avalabile balance for a buyer
    /// return a dictionary that represent the number of coins 
    /// </summary>
    /// <param name="buyerId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="SaveFaildExeption"></exception>
    public async Task<Result<Dictionary<int,int>>> WithdrawAllBalanceAsync(Guid buyerId, CancellationToken ct)
    {
        if (!await _buyerRepository.AnyAsync(b => b.Id == buyerId))
            return Result.Failure<Dictionary<int, int>>(Error.CreateFormExeption(new EntityNotFoundException(typeof(Buyer), buyerId)));

        int balance = await _financialTransactionRepository.GetBuyerBalanceAsync(buyerId, ct);
        if (balance <= 0)
            return Result.Failure<Dictionary<int, int>>(Error.CreateFormExeption(new InsufficantBalanceException()));
        // available Coins in the Machine 
        Dictionary<int, int> availableCoins = await _financialTransactionRepository.GetAvalibleCoinsAsync(ct);
        availableCoins = availableCoins.Where(c=>c.Value >0).OrderByDescending(x=>x.Key).ToDictionary();
        
        Dictionary<int, int> coinsToWithdraw = new Dictionary<int, int>();
        foreach (var coin in availableCoins)
        {
            int requierdCoinCount = balance / coin.Key;
            if(coin.Value >= requierdCoinCount)
            {
                coinsToWithdraw.Add(coin.Key, requierdCoinCount);
                balance = balance % coin.Key;
            }
            else
            {
                coinsToWithdraw.Add(coin.Key, coin.Value);
                balance = balance -( coin.Key*coin.Value);
            }
        }
        if (balance > 0)
            return Result.Failure<Dictionary<int, int>>(Error.CreateFormExeption(new InsufficantBalanceException()));

        foreach (var item in coinsToWithdraw)
        {
            _financialTransactionRepository.Insert(FinancialTransaction.Create(buyerId, FinancialTransactionType.Withdraw, item.Key, item.Value));
        }

        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 0)
            throw new SaveFaildExeption("Something went wrong , buyer data was not saved");

        return Result.Success(coinsToWithdraw);
    }

 
}
