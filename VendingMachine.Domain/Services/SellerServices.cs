using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Services;

public class SellerServices
{
    private readonly ISellerRepository _buyerRepository;

    private readonly IUnitOfWork _unitOfWork;
    public SellerServices(ISellerRepository buyerRepository, IUnitOfWork unitOfWork)
    {
        _buyerRepository = buyerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateSellerAsync(Seller buyer, CancellationToken ct)
    {
        if (buyer == null)
            throw new NullReferenceException("Seller Cannot be null ");
        if (!await _buyerRepository.IsNameUniqueAsync(buyer.Id, buyer.Name, ct))
            throw new DuplicateNameException(buyer.Name);
        _buyerRepository.Insert(buyer);
        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 0)
            throw new SaveFaildExeption("Something went wrong , buyer data was not saved");

        return buyer.Id;
    }
    public async Task<Result> UpdateSellerAsync(Guid id, string name, CancellationToken cancellationToken = default)
    {
        var existingSeller = await _buyerRepository.GetByIdAsync(id);

        if (existingSeller == null)
            return Result.Failure(Error.CreateFormExeption(new EntitiyNotFoundException(typeof(Seller), id)));

        var updateResult = existingSeller.Update(name);
        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Errors);


        if (!await _buyerRepository.IsNameUniqueAsync(id, name, cancellationToken))
            throw new DuplicateNameException(name);



        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
