using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Immutable;
using VendingMachine.Domain.Models;

namespace VendingMachine.Tests.Domain.Models;


[TestFixture]
public class FinancialTransactionTests
{
    [Test]
    public void Create_WithValidParameters_ShouldCreateFinancialTransactionSuccessfully()
    {
        // Arrange
        var buyerId = Guid.NewGuid();
        var transactionType = FinancialTransactionType.Deposit;
        int coin = 10;
        int coinCount = 5;

        // Act
        var transaction = FinancialTransaction.Create(buyerId, transactionType, coin, coinCount);

        // Assert
        transaction.Should().NotBeNull();
        transaction.BuyerId.Should().Be(buyerId);
        transaction.Coin.Should().Be(coin);
        transaction.CoinCount.Should().Be(coinCount);
        transaction.TransactionType.Should().Be(transactionType);
    }

    [Test]
    public void Create_WithInvalidCoin_ShouldThrowInvalidCoinException()
    {
        // Arrange
        var buyerId = Guid.NewGuid();
        var transactionType = FinancialTransactionType.Withdraw;
        int coin = 3; // Invalid coin value
        int coinCount = 1;

        // Act
        Action act = () => FinancialTransaction.Create(buyerId, transactionType, coin, coinCount);

        // Assert
        act.Should().Throw<InvalidCoinException>();
    }

    [Test]
    public void Create_WithInvalidCoinCount_ShouldThrowInvalidCoinCountException()
    {
        // Arrange
        var buyerId = Guid.NewGuid();
        var transactionType = FinancialTransactionType.Deposit;
        int coin = 10;
        int coinCount = 0; // Invalid coin count

        // Act
        Action act = () => FinancialTransaction.Create(buyerId, transactionType, coin, coinCount);

        // Assert
        act.Should().Throw<InvalidCoinCountException>();
    }
}

