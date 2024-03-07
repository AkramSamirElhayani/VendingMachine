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
public class BuyerTests
{
    [Test]
    public void Create_WithValidName_ShouldCreateBuyerSuccessfully()
    {
        // Arrange
        var name = "John Doe";

        // Act
        var buyer = Buyer.Create(name);

        // Assert
        buyer.Should().NotBeNull();
        buyer.Should().BeOfType<Buyer>();
        buyer.Name.Should().Be(name);
    }

    [Test]
    public void Create_WithNullName_ShouldThrowInvalidNameException()
    {
        // Arrange
        string name = null;

        // Act
        Action act = () => Buyer.Create(name);

        // Assert
        act.Should().Throw<InvalidNameExeption>();
    }

    [Test]
    public void Create_WithEmptyName_ShouldThrowInvalidNameException()
    {
        // Arrange
        var name = "";

        // Act
        Action act = () => Buyer.Create(name);

        // Assert
        act.Should().Throw<InvalidNameExeption>();
    }

    [Test]
    public void Update_WithValidName_ShouldUpdateNameSuccessfully()
    {
        // Arrange
        var initialName = "John Doe";
        var newName = "Jane Doe";
        var buyer = Buyer.Create(initialName);

        // Act
        var result = buyer.Update(newName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        buyer.Name.Should().Be(newName);
    }

    [Test]
    public void Update_WithNullName_ShouldThrowInvalidNameException()
    {
        // Arrange
        var buyer = Buyer.Create("John Doe");
        string newName = null;

        // Act
        Action act = () => buyer.Update(newName);

        // Assert
        act.Should().Throw<InvalidNameExeption>();
    }

    [Test]
    public void Update_WithEmptyName_ShouldThrowInvalidNameException()
    {
        // Arrange
        var buyer = Buyer.Create("John Doe");
        var newName = "";

        // Act
        Action act = () => buyer.Update(newName);

        // Assert
        act.Should().Throw<InvalidNameExeption>();
    }
}


