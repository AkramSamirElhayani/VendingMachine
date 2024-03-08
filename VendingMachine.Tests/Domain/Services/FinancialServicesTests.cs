using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Exeptions;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services;

namespace VendingMachine.Tests.Domain.Services;

public class FinancialServicesTests
{
    private Mock<IFinancialTransactionRepository> _financialTransactionRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IBuyerRepository> _buyerRepositoryMock;
    private Mock<IInventoryTransactionRepository> _inventoryTransactionRepositoryMock;
    private FinancialServices _financialServices;

    [SetUp]
    public void SetUp()
    {
        _financialTransactionRepositoryMock = new Mock<IFinancialTransactionRepository>();
        _inventoryTransactionRepositoryMock = new Mock<IInventoryTransactionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _buyerRepositoryMock = new Mock<IBuyerRepository>();
        _financialServices = new FinancialServices(_financialTransactionRepositoryMock.Object, _unitOfWorkMock.Object, _buyerRepositoryMock.Object, _inventoryTransactionRepositoryMock.Object);
    }
    [Test]
    public async Task DepositAsync_WithValidParameters_ShouldSucceed()
    {
        // Arrange
        var buyerId = Guid.NewGuid();
        Dictionary<int, int> curvals = new Dictionary<int, int> { { 100, 2 } }; // Simulate depositing two 100 coins

        _buyerRepositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Buyer, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1); // Simulate successful save

        // Act
        var result = await _financialServices.DepositAsync(buyerId, curvals, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _financialTransactionRepositoryMock.Verify(repo => repo.Insert(It.IsAny<FinancialTransaction>()), Times.Exactly(curvals.Count));
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    [Test]
    public async Task DepositAsync_WithNonExistingBuyer_ShouldFail()
    {
        // Arrange
        var buyerId = Guid.NewGuid();
        Dictionary<int, int> curvals = new Dictionary<int, int> { { 50, 1 } };

        _buyerRepositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Buyer, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false); // Simulate buyer does not exist

        // Act
        var result = await _financialServices.DepositAsync(buyerId, curvals, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == nameof(EntityNotFoundException));
    }
    [Test]
    public async Task WithDrawAllBalance_WithSufficientBalanceAndAvailableCoins_ShouldSucceed()
    {
        // Arrange
        var buyerId = Guid.NewGuid();
        int initialBalance = 150; // Assuming the balance is 150
        Dictionary<int, int> availableCoins = new Dictionary<int, int> { { 100, 1 }, { 50, 1 } }; // Simulate machine state

        _buyerRepositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Buyer, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _financialTransactionRepositoryMock.Setup(repo => repo.GetBuyerBalanceAsync(buyerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(initialBalance);

        _financialTransactionRepositoryMock.Setup(repo => repo.GetAvalibleCoinsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(availableCoins);

        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _financialServices.WithdrawAllBalanceAsync(buyerId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Equal(new Dictionary<int, int> { { 100, 1 }, { 50, 1 } }); // Expect to withdraw one 100 coin and one 50 coin
        _financialTransactionRepositoryMock.Verify(repo => repo.Insert(It.IsAny<FinancialTransaction>()), Times.Exactly(2));
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task WithDrawAllBalance_InsufficientCoinDenominations_ShouldFail()
    {
        // Arrange
        var buyerId = Guid.NewGuid();
        int initialBalance = 80; // Assuming the balance is 80
                                 // Machine has only denominations that don't allow for exact withdrawal
                                 // e.g., only has coins of 100, which can't be used to withdraw a balance of 80
        Dictionary<int, int> availableCoins = new Dictionary<int, int> { { 100, 2 } };

        _buyerRepositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Buyer, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _financialTransactionRepositoryMock.Setup(repo => repo.GetBuyerBalanceAsync(buyerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(initialBalance);

        _financialTransactionRepositoryMock.Setup(repo => repo.GetAvalibleCoinsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(availableCoins);

        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _financialServices.WithdrawAllBalanceAsync(buyerId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        // The failure is due to an inability to provide exact change
        result.Errors.Should().Contain(e => e.Code == "InsufficantBalanceException" || e.Message.Contains("Unable to provide exact change due to coin denomination limitations"));
        _financialTransactionRepositoryMock.Verify(repo => repo.Insert(It.IsAny<FinancialTransaction>()), Times.Never);
        // Verify that no changes are attempted to be saved due to the failure
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }


}
