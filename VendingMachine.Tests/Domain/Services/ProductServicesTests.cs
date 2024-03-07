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
public class ProductServicesTests
{
    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<IInventoryTransactionRepository> _inventoryTransactionRepositoryMock;
    private   Mock<IUnitOfWork> _unitOfWorkMock ;
    private   ProductServices _productServices;

 


    [SetUp]
    public void SetUp()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _inventoryTransactionRepositoryMock = new Mock<IInventoryTransactionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productServices = new ProductServices(_productRepositoryMock.Object, _unitOfWorkMock.Object, _inventoryTransactionRepositoryMock.Object);
    }

 
    [Test]
    public async Task CreateProductAsync_WithValidProduct_ShouldReturnProductId()
    {
        // Arrange
        var product = Product.Create("TestProduct", 100, Guid.NewGuid(), "Description");
        _productRepositoryMock.Setup(repo => repo.IsNameUniqueAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny <CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _productServices.CreateProductAsync(product, CancellationToken.None);

        // Assert
        result.Should().Be(product.Id);
        _productRepositoryMock.Verify(pr => pr.Insert(product), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    [Test]
    public async Task UpdateProductInfoAsync_WithValidData_ShouldUpdateProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var existingProduct = Product.Create("TestProduct", 100, Guid.NewGuid(), "Old Description");
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId, It.IsAny<CancellationToken>())).ReturnsAsync(existingProduct);
        _productRepositoryMock.Setup(repo => repo.IsNameUniqueAsync(productId, It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _productServices.UpdateProductInfoAsync(productId, "UpdatedName", "Updated Description");

        // Assert
        result.IsSuccess.Should().BeTrue();
        existingProduct.Should().BeEquivalentTo(new { Name = "UpdatedName", Description = "Updated Description" });
    }
    [Test]
    public async Task AddProductToInventory_WithValidParameters_ShouldReturnNewBalance()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var countToAdd = 10;
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId, It.IsAny<CancellationToken>())).ReturnsAsync(Product.Create("TestProduct", 100, Guid.NewGuid(), "Description"));
        _inventoryTransactionRepositoryMock.Setup(repo => repo.GetProductBalanceAsync(productId, It.IsAny<CancellationToken>())).ReturnsAsync(20); // Assume new balance is 20
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _productServices.AddProductToInventory(productId, countToAdd);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(20); // The expected new balance
    }


}

