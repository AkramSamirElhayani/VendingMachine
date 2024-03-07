using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services;

namespace VendingMachine.Tests.Domain.Services;
public class SellerServicesTests
{
    private readonly Mock<ISellerRepository> _sellerRepositoryMock = new Mock<ISellerRepository>();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
    private readonly SellerServices _sellerServices;

    public SellerServicesTests()
    {
        _sellerServices = new SellerServices(_sellerRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task CreateSellerAsync_WithValidSeller_ShouldReturnSellerId()
    {
        // Arrange
        var seller = Seller.Create("TestSeller");
        _sellerRepositoryMock.Setup(repo => repo.IsNameUniqueAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _sellerServices.CreateSellerAsync(seller, CancellationToken.None);

        // Assert
        result.Should().Be(seller.Id);
        _sellerRepositoryMock.Verify(sr => sr.Insert(seller), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    [Test]
    public async Task UpdateSellerAsync_WithValidData_ShouldUpdateSeller()
    {
        // Arrange
        
        var existingSeller = Seller.Create("TestSeller");
        _sellerRepositoryMock.Setup(repo => repo.GetByIdAsync(existingSeller.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existingSeller);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _sellerServices.UpdateSellerAsync(existingSeller.Id, "UpdatedSellerName");

        // Assert
        result.IsSuccess.Should().BeTrue();
        existingSeller.Should().BeEquivalentTo(new { Name = "UpdatedSellerName" });
    }


}
