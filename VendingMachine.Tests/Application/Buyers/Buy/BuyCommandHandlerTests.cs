using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Buyers.Command.Buy;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services;

namespace VendingMachine.Tests.Application.Buyers.Buy;

public class BuyCommandHandlerTests
{
    private Mock<IProductRepository> mockProductRepository;
    private Mock<IFinancialServices> mockFinancialServices;
    private Mock<IUnitOfWork> mockUnitOfWork;
    private Mock<IProductServices> mockProductServices;
    private Mock<IInventoryTransactionRepository> mockInventoryTransactionRepository;


    [SetUp]
    public void SetUp()
    {
        mockProductRepository = new Mock<IProductRepository>();
        mockFinancialServices = new Mock<IFinancialServices>();
        mockUnitOfWork = new Mock<IUnitOfWork>();
        mockProductServices = new Mock<IProductServices>();
        mockInventoryTransactionRepository = new Mock<IInventoryTransactionRepository>();
    }


    [Test]
    public async Task Handle_WhenProductNotFound_ShouldReturnFailure()
    {
        // Arrange
    
        var handler = new BuyCommandHandler(mockProductServices.Object, mockFinancialServices.Object, mockUnitOfWork.Object, mockProductRepository.Object, mockInventoryTransactionRepository.Object);
        var command = new BuyCommand(Guid.NewGuid(), Guid.NewGuid(), 1);

        mockProductRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Code == nameof(EntityNotFoundException));
    }
    [Test]
    public async Task Handle_WhenBuyerBalanceRetrievalFails_ShouldReturnFailure()
    {
        // Arrange
        var error = new Error("", "Error retrieving balance");
        var product = Product.Create("TestProduct", 100, Guid.NewGuid(), "Description");
        var handler = new BuyCommandHandler(mockProductServices.Object, mockFinancialServices.Object, mockUnitOfWork.Object, mockProductRepository.Object, mockInventoryTransactionRepository.Object);
        mockFinancialServices.Setup(x => x.GetBuyerBalanceAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<int>(error));
        mockProductRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(product);
        var command = new BuyCommand(Guid.NewGuid(), Guid.NewGuid(), 1); // Assuming constructor parameters

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(error);
    }
    [Test]
    public async Task Handle_WhenBuyerHasInsufficientBalance_ShouldReturnFailure()
    {
        // Arrange
        var handler = new BuyCommandHandler(mockProductServices.Object, mockFinancialServices.Object, mockUnitOfWork.Object, mockProductRepository.Object, mockInventoryTransactionRepository.Object);
        var product = Product.Create("TestProduct", 100, Guid.NewGuid(), "Description");
        // Mocking product to return a product with a price
        mockProductRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);  

        mockFinancialServices.Setup(x => x.GetBuyerBalanceAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(5)); // Assuming the product price is higher than 5

        var command = new BuyCommand(Guid.NewGuid(), Guid.NewGuid(), 2); 

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Code == "InsufficantBalanceException");
    }
    [Test]
    public async Task Handle_WhenDespensingProductFails_ShouldReturnFailureAndRollbackTransaction()
    {
        // Arrange
        var error = new Error("", "Despense failed");
        var handler = new BuyCommandHandler(mockProductServices.Object, mockFinancialServices.Object, mockUnitOfWork.Object, mockProductRepository.Object, mockInventoryTransactionRepository.Object);
        var command = new BuyCommand(Guid.NewGuid(), Guid.NewGuid(), 1);
        var product = Product.Create("TestProduct", 100, Guid.NewGuid(), "Description");
        // Simulate product found and sufficient balance before testing despense failure
        mockProductRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product); 

        mockFinancialServices.Setup(x => x.GetBuyerBalanceAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(100)); // Assuming balance is sufficient

        mockProductServices.Setup(x => x.Despense(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(error));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(error);
        mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    [Test]
    public async Task Handle_WhenBalanceIsMoreThanEnough_ShouldReturnSuccessWithChange()
    {
        // Arrange
        var product = Product.Create("TestProduct", 200, Guid.NewGuid(), "Description");
        var productId = product.Id;
        var buyerId = Guid.NewGuid();
        var buyerBalance = 1200; // Buyer has more balance than needed
        var amountToPurchase = 1;
        var expectedChange = new Dictionary<int, int> { { 100, 1 } }; // Example change

        mockProductRepository.Setup(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        mockFinancialServices.Setup(x => x.GetBuyerBalanceAsync(buyerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(buyerBalance));

        mockFinancialServices.Setup(x => x.WithdrawAllBalanceAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedChange));


        mockProductServices.Setup(x => x.Despense(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var buyCommand = new BuyCommand(buyerId, productId, amountToPurchase);
        var handler = new BuyCommandHandler(mockProductServices.Object, mockFinancialServices.Object, mockUnitOfWork.Object, mockProductRepository.Object, mockInventoryTransactionRepository.Object);
        // Act
        var result = await handler.Handle(buyCommand, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Change.Should().BeEquivalentTo(expectedChange);
    }
    [Test]
    public async Task Handle_WhenWithdrawingBalanceForChangeFails_ShouldReturnFailure()
    {
        // Arrange 
        var error = new Error("", "Despense failed");
        var product = Product.Create("TestProduct", 200, Guid.NewGuid(), "Description");
        var productId = product.Id;
        var buyerId = Guid.NewGuid();
        
        mockProductRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        mockFinancialServices.Setup(x => x.WithdrawAllBalanceAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<Dictionary<int, int>>(error));
        mockFinancialServices.Setup(x => x.GetBuyerBalanceAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(1000));
        mockProductServices.Setup(x => x.Despense(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(Result.Success());

        var buyCommand = new BuyCommand(buyerId, productId, 1);
        var handler = new BuyCommandHandler(mockProductServices.Object, mockFinancialServices.Object, mockUnitOfWork.Object, mockProductRepository.Object, mockInventoryTransactionRepository.Object);

        // Act
        var result = await handler.Handle(buyCommand, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(error);
        mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    [Test]
    public async Task Handle_WhenExactBalance_ShouldCompletePurchaseWithoutChange()
    {
        // Arrange 
        var product = Product.Create("TestProduct", 200, Guid.NewGuid(), "Description");
        var productId = product.Id;
        var buyerId = Guid.NewGuid();
        var amountToPurchase = 1;
        var exactBalance = product.Price * amountToPurchase;

        mockFinancialServices.Setup(x => x.GetBuyerBalanceAsync(buyerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(exactBalance));

        mockProductServices.Setup(x => x.Despense(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(Result.Success());

        mockProductRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
         .ReturnsAsync(product);

        var buyCommand = new BuyCommand(buyerId, productId, 1);
        var handler = new BuyCommandHandler(mockProductServices.Object, mockFinancialServices.Object, mockUnitOfWork.Object, mockProductRepository.Object, mockInventoryTransactionRepository.Object);

        // Act
        var result = await handler.Handle(buyCommand, CancellationToken.None);


        //Assert, expect success and no change
        result.IsSuccess.Should().BeTrue(); 
        result.Value.Change.Should().BeEmpty();
    }

    [Test]
    public async Task Handle_WhenAllOperationsSucceed_ShouldCommitTransaction()
    {
        // Arrange
        // Arrange 
        var product = Product.Create("TestProduct", 200, Guid.NewGuid(), "Description");
        var productId = product.Id;
        var buyerId = Guid.NewGuid();
        var amountToPurchase = 1;
        var exactBalance = product.Price * amountToPurchase;
        mockFinancialServices.Setup(x => x.GetBuyerBalanceAsync(buyerId, It.IsAny<CancellationToken>()))
           .ReturnsAsync(Result.Success(1000));

        mockFinancialServices.Setup(x => x.WithdrawAllBalanceAsync(productId, It.IsAny<CancellationToken>()))
          .ReturnsAsync(Result.Success(new Dictionary<int, int>()));

        mockProductServices.Setup(x => x.Despense(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(Result.Success());

        mockProductRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
         .ReturnsAsync(product);



        var buyCommand = new BuyCommand(buyerId, productId, 1);
        var handler = new BuyCommandHandler(mockProductServices.Object, mockFinancialServices.Object, mockUnitOfWork.Object, mockProductRepository.Object, mockInventoryTransactionRepository.Object);


        // Act
        var result = await handler.Handle(buyCommand, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        mockUnitOfWork.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    [Test]
    public async Task Handle_WhenPurchaseIsSuccessful_ShouldReturnAccurateTotalSpent()
    {
        // Arrange 
        var product = Product.Create("TestProduct", 100, Guid.NewGuid(), "Description");
        var buyerId = Guid.NewGuid();
        var productId = product.Id;
        var productPrice = product.Price;
        var amountToPurchase = 2;
        var buyCommand = new BuyCommand(buyerId, productId, amountToPurchase);

        var handler = new BuyCommandHandler(mockProductServices.Object, mockFinancialServices.Object, mockUnitOfWork.Object, mockProductRepository.Object, mockInventoryTransactionRepository.Object);


        mockProductRepository.Setup(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        mockFinancialServices.Setup(x => x.GetBuyerBalanceAsync(buyerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(productPrice * amountToPurchase));

        mockProductServices.Setup(x => x.Despense(productId, amountToPurchase, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        mockFinancialServices.Setup(x=>x.GetTotalSoldProductsPriceSumAsync(buyerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(productPrice * amountToPurchase);


        var expectedTotalSpent = product.Price * amountToPurchase; // Adjust based on setup

        // Act
        var result = await handler.Handle(buyCommand, CancellationToken.None);


        // Assert
        result.Value.TotalSpent.Should().Be(expectedTotalSpent);
    }

}
