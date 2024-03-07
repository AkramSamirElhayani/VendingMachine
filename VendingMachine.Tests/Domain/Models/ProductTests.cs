using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using FluentAssertions;
using NUnit.Framework;
using VendingMachine.Domain.Exeptions;
using VendingMachine.Domain.Models;

namespace VendingMachine.Tests.Domain.Models;


    [TestFixture]
    public class ProductTests
    {
        [Test]
        public void Create_WithValidParameters_ShouldCreateProductSuccessfully()
        {
            // Arrange
            string name = "Test Product";
            int price = FinancialTransaction.AllowedCoins.Min(); // Ensuring price is valid
            var sellerId = Guid.NewGuid();
            string description = "Test Description";

            // Act
            var product = Product.Create(name, price, sellerId, description);

            // Assert
            product.Should().NotBeNull();
            product.Name.Should().Be(name);
            product.Price.Should().Be(price);
            product.SellerId.Should().Be(sellerId);
            product.Description.Should().Be(description);
        }

        [Test]
        public void Create_WithEmptyName_ShouldThrowInvalidNameException()
        {
            // Arrange
            string name = "";
            int price = FinancialTransaction.AllowedCoins.Min();
            var sellerId = Guid.NewGuid();
            string description = "Test Description";

            // Act
            Action act = () => Product.Create(name, price, sellerId, description);

            // Assert
            act.Should().Throw<InvalidNameExeption>();
        }

        [Test]
        public void Create_WithInvalidPrice_ShouldThrowInvalidPriceException()
        {
            // Arrange
            string name = "Test Product";
            int price = FinancialTransaction.AllowedCoins.Min() - 1; // Invalid price
            var sellerId = Guid.NewGuid();
            string description = "Test Description";

            // Act
            Action act = () => Product.Create(name, price, sellerId, description);

            // Assert
            act.Should().Throw<InvalidPriceExeption>();
        }

        [Test]
        public void Update_WithValidParameters_ShouldUpdateProductSuccessfully()
        {
            // Arrange
            var product = Product.Create("Initial Name", FinancialTransaction.AllowedCoins.Min(), Guid.NewGuid(), "Initial Description");
            string updatedName = "Updated Name";
            string updatedDescription = "Updated Description";

            // Act
            var result = product.Update(updatedName, updatedDescription);

            // Assert
            result.IsSuccess.Should().BeTrue();
            product.Name.Should().Be(updatedName);
            product.Description.Should().Be(updatedDescription);
        }

        [Test]
        public void UpdatePrice_WithValidPrice_ShouldUpdatePriceSuccessfully()
        {
            // Arrange
            var product = Product.Create("Product Name", FinancialTransaction.AllowedCoins.Min(), Guid.NewGuid(), "Description");
            int updatedPrice = FinancialTransaction.AllowedCoins.Min() * 2;

            // Act
            var result = product.UpdatePrice(updatedPrice);

            // Assert
            result.IsSuccess.Should().BeTrue();
            product.Price.Should().Be(updatedPrice);
        }

        [Test]
        public void UpdatePrice_WithInvalidPrice_ShouldThrowInvalidPriceException()
        {
            // Arrange
            var product = Product.Create("Product Name", FinancialTransaction.AllowedCoins.Min(), Guid.NewGuid(), "Description");
            int invalidPrice = FinancialTransaction.AllowedCoins.Min() - 1; // Ensuring price is invalid

            // Act
            Action act = () => product.UpdatePrice(invalidPrice);

            // Assert
            act.Should().Throw<InvalidPriceExeption>();
        }
    }

