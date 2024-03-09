using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using FluentAssertions;
using NUnit.Framework;
using VendingMachine.Domain.Models;

namespace VendingMachine.Tests.Domain.Models;


[TestFixture]
public class InventoryTransactionTests
{
    [Test]
    public void Create_WithNonPositiveCount_ShouldThrowArgumentException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var transactionType = InventoryTransactionType.Add;
        int count = 0;
        int price = 10;

        // Act
        Action act = () => InventoryTransaction.Create(productId, transactionType, count,price);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Count Must be more than zero (Parameter 'count')");
    
    }
    [Test]
    public void Create_WithNonPositivePrice_ShouldThrowArgumentException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var transactionType = InventoryTransactionType.Add;
        int count = 5;
        int price = 0;

        // Act
        Action act = () => InventoryTransaction.Create(productId, transactionType, count, price);

        // Assert
        act.Should().Throw<ArgumentException>(); 

    }
    [Test]
    public void Create_WithValidCount_ShouldCreateInventoryTransactionSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var transactionType = InventoryTransactionType.Add;
        int count = 5;
        int price = 10;


        // Act
        var transaction = InventoryTransaction.Create(productId, transactionType, count, price);

        // Assert
        transaction.Should().NotBeNull();
        transaction.ProductId.Should().Be(productId);
        transaction.Count.Should().Be(count);
        transaction.TransactionType.Should().Be(transactionType);
        transaction.Date.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1)); // Allows a small leeway in timing
    }


}

