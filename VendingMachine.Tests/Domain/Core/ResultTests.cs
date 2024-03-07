using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using VendingMachine.Domain.Core;

namespace VendingMachine.Tests.Domain.Core;

[TestFixture]
public class ResultTests
{
    [Test]
    public void Success_ShouldReturnSuccessResult()
    {
        // Arrange
        var result = Result.Success();

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(ResultType.Success);
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void Success_ShouldReturnSuccessResultWithValue()
    {
        // Arrange
        var value = 42;
        var result = Result.Success(value);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(ResultType.Success);
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Errors.Should().BeEmpty();
        result.Value.Should().Be(value);
    }

    [Test]
    public void Failure_ShouldReturnFailureResultWithErrors()
    {
        // Arrange
        var errors = new[] { new Error("Error 1", "Error Message 1"), new Error("Error 2", "Error Message 2") };
        var result = Result.Failure(errors);

        // Assert
        result.Should().NotBeNull();
        Assert.That(result.Status, Is.EqualTo(ResultType.Failure));
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        Assert.That(result.Errors.Length, Is.EqualTo(errors.Length));
        Assert.That(result.Errors, Is.EqualTo(errors));
    }

    [Test]
    public void Failure_ShouldReturnFailureResultWithValueAndErrors()
    {
        // Arrange
        var errors = new[] { new Error("Error 1", "Error Message 1"), new Error("Error 2", "Error Message 2") };
        var result = Result.Failure<int>(errors);

        // Assert
        result.Should().NotBeNull();
        Assert.That(result.Status, Is.EqualTo(ResultType.Failure));
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        Assert.That(result.Errors.Length, Is.EqualTo(errors.Length));
        Assert.That(result.Errors, Is.EqualTo(errors));
        Assert.That(result.Value, Is.EqualTo(default(int)));
    }

    [Test]
    public void FirstFailureOrSuccess_ShouldReturnSuccess_WhenAllResultsAreSuccess()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Success();

        // Act
        var result = Result.FirstFailureOrSuccess(result1, result2);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(ResultType.Success);
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void FirstFailureOrSuccess_ShouldReturnFirstFailure_WhenAtLeastOneResultIsFailure()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Failure(new Error("Error 1", "Error Message 2"));
        var result3 = Result.Failure(new Error("Error 2", "Error Message 2"));

        // Act
        var result = Result.FirstFailureOrSuccess(result1, result2, result3);

        // Assert
        result.Should().NotBeNull();
        Assert.That(result.Status, Is.EqualTo(ResultType.Failure));
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        Assert.That(result.Errors.Length, Is.EqualTo(1));
        Assert.That(result.Errors[0], Is.EqualTo(result2.Errors[0]));
    }
}
