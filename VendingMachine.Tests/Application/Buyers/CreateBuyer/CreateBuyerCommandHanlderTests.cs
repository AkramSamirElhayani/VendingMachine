using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Applicaion.Buyers.Command.CreateBuyer;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services;

namespace VendingMachine.Tests.Application.Buyers.CreateBuyer;


[TestFixture]
public class CreateBuyerCommandHandlerTests
{
    private CreateBuyerCommandHandler _handler;
    private Mock<IBuyerServices> _mockBuyerServices;

    [SetUp]
    public void SetUp()
    {
        _mockBuyerServices = new Mock<IBuyerServices>();
        _handler = new CreateBuyerCommandHandler(_mockBuyerServices.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccessWithNewBuyerId_WhenBuyerIsCreated()
    {
        // Arrange
        var buyerName = "Test Buyer";
        var newBuyerId = Guid.NewGuid();
        var createBuyerCommand = new CreateBuyerCommand( buyerName );

        _mockBuyerServices.Setup(s => s.CreateBuyerAsync(It.IsAny<Buyer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newBuyerId);

        // Act
        var result = await _handler.Handle(createBuyerCommand, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(newBuyerId);
        _mockBuyerServices.Verify(s => s.CreateBuyerAsync(It.IsAny<Buyer>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}