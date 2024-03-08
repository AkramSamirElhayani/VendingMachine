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
    private readonly ISellerRepository _sellerRepository;

    private readonly IUnitOfWork _unitOfWork;
    public SellerServices(ISellerRepository sellerRepository, IUnitOfWork unitOfWork)
    {
        _sellerRepository = sellerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateSellerAsync(Seller seller, CancellationToken ct)
    {
        if (seller == null)
            throw new NullReferenceException("Seller Cannot be null ");
        if (!await _sellerRepository.IsNameUniqueAsync(seller.Id, seller.Name, ct))
            throw new DuplicateNameException(seller.Name);
        _sellerRepository.Insert(seller);
        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 0)
            throw new SaveFaildExeption("Something went wrong , seller data was not saved");

        return seller.Id;
    }
    public async Task<Result> UpdateSellerAsync(Guid id, string name, CancellationToken cancellationToken = default)
    {
        var existingSeller = await _sellerRepository.GetByIdAsync(id);

        if (existingSeller == null)
            return Result.Failure(Error.CreateFormExeption(new EntityNotFoundException(typeof(Seller), id)));

        var updateResult = existingSeller.Update(name);
        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Errors);


        if (!await _sellerRepository.IsNameUniqueAsync(id, name, cancellationToken))
            throw new DuplicateNameException(name);



        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }



}
