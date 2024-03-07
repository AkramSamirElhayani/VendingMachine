using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;

namespace VendingMachine.Tests.Domain.Core;

[TestFixture]
public class ValueObjectTests
{
    private class TestValueObject : ValueObject<TestValueObject>
    {
        private readonly string _value1;
        private readonly int _value2;

        public TestValueObject(string value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _value1;
            yield return _value2;
        }
    }

    [Test]
    public void Equals_ReturnsTrue_WhenObjectsAreEqual()
    {
        // Arrange
        var obj1 = new TestValueObject("test", 123);
        var obj2 = new TestValueObject("test", 123);

        // Act
        var result = obj1.Equals(obj2);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Equals_ReturnsFalse_WhenObjectsAreNotEqual()
    {
        // Arrange
        var obj1 = new TestValueObject("test1", 123);
        var obj2 = new TestValueObject("test2", 123);

        // Act
        var result = obj1.Equals(obj2);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void GetHashCode_ReturnsSameValue_WhenObjectsAreEqual()
    {
        // Arrange
        var obj1 = new TestValueObject("test", 123);
        var obj2 = new TestValueObject("test", 123);

        // Act
        var hash1 = obj1.GetHashCode();
        var hash2 = obj2.GetHashCode();

        // Assert
        Assert.That(hash2, Is.EqualTo(hash1));
    }

    [Test]
    public void GetHashCode_ReturnsDifferentValue_WhenObjectsAreNotEqual()
    {
        // Arrange
        var obj1 = new TestValueObject("test1", 123);
        var obj2 = new TestValueObject("test2", 123);

        // Act
        var hash1 = obj1.GetHashCode();
        var hash2 = obj2.GetHashCode();

        // Assert
        Assert.That(hash2, Is.Not.EqualTo(hash1));
    }

    [Test]
    public void OperatorEquals_ReturnsTrue_WhenObjectsAreEqual()
    {
        // Arrange
        var obj1 = new TestValueObject("test", 123);
        var obj2 = new TestValueObject("test", 123);

        // Act
        var result = obj1 == obj2;

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void OperatorEquals_ReturnsFalse_WhenObjectsAreNotEqual()
    {
        // Arrange
        var obj1 = new TestValueObject("test1", 123);
        var obj2 = new TestValueObject("test2", 123);

        // Act
        var result = obj1 == obj2;

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void OperatorNotEquals_ReturnsTrue_WhenObjectsAreNotEqual()
    {
        // Arrange
        var obj1 = new TestValueObject("test1", 123);
        var obj2 = new TestValueObject("test2", 123);

        // Act
        var result = obj1 != obj2;

        // Assert
        result.Should().BeTrue();
    }


    [Test]
    public void OperatorNotEquals_ReturnsFalse_WhenObjectsAreEqual()
    {
        // Arrange
        var obj1 = new TestValueObject("test", 123);
        var obj2 = new TestValueObject("test", 123);

        // Act
        var result = obj1 != obj2;

        // Assert
        result.Should().BeFalse();
    }
}