using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Exeptions;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Services;

public class ProductServices: IProductServices
{
    private readonly IProductRepository _productRepository; 
    private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISellerRepository _sellerRepository;

    public ProductServices(IProductRepository productRepository, IUnitOfWork unitOfWork, IInventoryTransactionRepository inventoryTransactionRepository, ISellerRepository sellerRepository)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _sellerRepository = sellerRepository;
    }
    public async Task<Guid> CreateProductAsync(Product product, CancellationToken ct)
    {
        if (product == null)
            throw new NullReferenceException("Product Cannot be null ");
        if (!await _productRepository.IsNameUniqueAsync(product.Id, product.Name, ct))
            throw new DuplicateNameException(product.Name);
        if (!await _sellerRepository.AnyAsync(s => s.Id == product.SellerId))
            throw new EntityNotFoundException(typeof(Seller), product.Id);

        _productRepository.Insert(product);
        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 0)
            throw new SaveFaildExeption("Something went wrong , product data was not saved");

        return product.Id;
    }
    public async Task<Result> UpdateProductInfoAsync(Guid id, string name,string? description, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);

        if (existingProduct == null)
            return Result.Failure(Error.CreateFormExeption(new EntityNotFoundException(typeof(Product), id)));

        var updateResult = existingProduct.Update(name, description);
        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Errors);


        if (!await _productRepository.IsNameUniqueAsync(id, name, cancellationToken))
            throw new DuplicateNameException(name);

        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 0)
            throw new SaveFaildExeption("Something went wrong , product data was not saved");

        return Result.Success();
    }
    public async Task<Result> UpdateProductPriceAsync(Guid id, int price, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);

        if (existingProduct == null)
            return Result.Failure(Error.CreateFormExeption(new EntityNotFoundException(typeof(Product), id)));

        var updateResult = existingProduct.UpdatePrice(price);
        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Errors);


        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 0)
            throw new SaveFaildExeption("Something went wrong , product data was not saved");

        return Result.Success();
    }

    /// <summary>
    /// Add Quantity of a product to the Machine and return the new Product Balance
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="count"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<Result<int>> AddProductToInventory(Guid productId , int count ,CancellationToken ct = default)
    {
        var existingProduct = await _productRepository.GetByIdAsync(productId);

        if (existingProduct == null)
            return Result.Failure<int>(Error.CreateFormExeption(new EntityNotFoundException(typeof(Product), productId)));

        _inventoryTransactionRepository.Insert(InventoryTransaction.Create(productId,InventoryTransactionType.Add, count, existingProduct.Price));
        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 0)
            throw new SaveFaildExeption("Something went wrong , Inventory Transaction was not created");

        return await _inventoryTransactionRepository.GetProductBalanceAsync(productId, ct);

    }

    public async Task<Result> Despense(Guid productId, int count, CancellationToken ct = default)
    {
        var existingProduct = await _productRepository.GetByIdAsync(productId);

        if (existingProduct == null)
            return Result.Failure<int>(Error.CreateFormExeption(new EntityNotFoundException(typeof(Product), productId)));

        if (await _inventoryTransactionRepository.GetProductBalanceAsync(productId, ct) < count)
            return Result.Failure<int>(Error.CreateFormExeption(new InsufficantProductBalanceException()));

        _inventoryTransactionRepository.Insert(InventoryTransaction.Create(productId, InventoryTransactionType.Remove, count, existingProduct.Price));

        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 0)
            throw new SaveFaildExeption("Something went wrong , Inventory Transaction was not created");

        return Result.Success();

    }
    public async Task<Result> DeleteProductAsync(Guid productId,CancellationToken cancellationToken)
    {
        var existingProduct = await _productRepository.GetByIdAsync(productId);

        if (existingProduct == null)
            return Result.Failure(Error.CreateFormExeption(new EntityNotFoundException(typeof(Product), productId)));

        _productRepository.Remove(existingProduct);
         
        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 0)
            throw new SaveFaildExeption("Something went wrong , product was not deleted");

        return Result.Success();

    }

}
