using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services;


namespace VendingMachine.Tests.Domain.Services;

[TestFixture]
public class BuyerServicesTests
{
    [Test]
    public async Task CreateBuyerAsync_WithValidBuyer_ShouldCreateBuyerSuccessfully()
    {
        // Arrange
        var buyerRepositoryMock = new Mock<IBuyerRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var buyer = Buyer.Create("John Doe");
        buyerRepositoryMock.Setup(repo => repo.IsNameUniqueAsync(buyer.Id, buyer.Name, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        unitOfWorkMock.Setup(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var buyerServices = new BuyerServices(buyerRepositoryMock.Object, unitOfWorkMock.Object);

        // Act
        Func<Task> act = async () => await buyerServices.CreateBuyerAsync(buyer, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task CreateBuyerAsync_WithNullBuyer_ShouldThrowNullReferenceException()
    {
        // Arrange
        var buyerRepositoryMock = new Mock<IBuyerRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var buyerServices = new BuyerServices(buyerRepositoryMock.Object, unitOfWorkMock.Object);

        // Act
        Func<Task> act = async () => await buyerServices.CreateBuyerAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    [Test]
    public async Task UpdateBuyerAsync_WithValidBuyer_ShouldUpdateBuyerSuccessfully()
    {
        // Arrange
        var existingBuyer =  Buyer.Create("John Doe");
        var buyerId = existingBuyer.Id;
        var buyerNewName = "Jane Doe";

        var buyerRepositoryMock = new Mock<IBuyerRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();


        buyerRepositoryMock.Setup(repo => repo.GetByIdAsync(buyerId, It.IsAny<CancellationToken>())).ReturnsAsync(existingBuyer);
        buyerRepositoryMock.Setup(repo => repo.IsNameUniqueAsync(buyerId, buyerNewName, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        unitOfWorkMock.Setup(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var buyerServices = new BuyerServices(buyerRepositoryMock.Object, unitOfWorkMock.Object);

        // Act
        var result = await buyerServices.UpdateBuyerAsync(buyerId, "Jane Doe", CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
    [Test]
    public async Task UpdateBuyerAsync_WithNonexistentBuyer_ShouldReturnEntityNotFoundException()
    {
        // Arrange
        var buyerId = Guid.NewGuid();
        var buyerRepositoryMock = new Mock<IBuyerRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        buyerRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Buyer)null);

        var buyerServices = new BuyerServices(buyerRepositoryMock.Object, unitOfWorkMock.Object);

        // Act
        var result = await buyerServices.UpdateBuyerAsync(buyerId, "New Name");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Code == nameof(EntityNotFoundException));
    }
    [Test]
    public async Task UpdateBuyerAsync_WhenSaveFails_ShouldThrowSaveFailedException()
    {
        // Arrange
        var buyerId = Guid.NewGuid();
        var existingBuyer = Buyer.Create("Existing Name");
        var buyerRepositoryMock = new Mock<IBuyerRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        buyerRepositoryMock.Setup(repo => repo.GetByIdAsync(buyerId, It.IsAny<CancellationToken>())).ReturnsAsync(existingBuyer);
        buyerRepositoryMock.Setup(repo => repo.IsNameUniqueAsync(buyerId, "New Name", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        unitOfWorkMock.Setup(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0); // Simulate save failure

        var buyerServices = new BuyerServices(buyerRepositoryMock.Object, unitOfWorkMock.Object);

        // Act
        Func<Task> act = async () => await buyerServices.UpdateBuyerAsync(buyerId, "New Name");

        // Assert
        await act.Should().ThrowAsync<SaveFaildExeption>();
    }


}

