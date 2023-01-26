using FluentAssertions;

namespace AocLib.Tests;

public class PointTests
{

    [Theory]
    [MemberData(nameof(EqualityTrueData))]
    public void Equality(Point p1, Point p2, bool result)
        => (p1 == p2).Should().Be(result);

    public static IEnumerable<object[]> EqualityTrueData
    => new List<object[]>
    {
            new object[] { new Point(1, 1), new Point(1, 0), false },
            new object[] { new Point(1, 1), new Point(0, 1), false },
            new object[] { new Point(1, 1), new Point(1, 1), true }
    };

    [Fact]
    public void Addition()
    {
        // Arrange
        var point = new Point(1, 1);

        // Act
        var result = point + new Point(2, 3);

        // Assert
        result.Should().Be(new Point(3, 4));
    }

    [Fact]
    public void Subtraction()
    {
        // Arrange
        var point = new Point(1, 1);

        // Act
        var result = point - new Point(2, 3);

        // Assert
        result.Should().Be(new Point(-1, -2));
    }

    [Fact]
    public void Multiplication()
    {
        // Arrange
        var point = new Point(2, 3);

        // Act
        var result = point * new Point(3, 4);

        // Assert
        result.Should().Be(new Point(6, 12));
    }

    [Fact]
    public void Division()
    {
        // Arrange
        var point = new Point(4, 12);

        // Act
        var result = point / new Point(2, 4);

        // Assert
        result.Should().Be(new Point(2, 3));
    }

    [Fact]
    public void Deconstruct()
    {
        // Arrange
        var point = new Point(4, 12);

        // Act
        var (x, y) = point;

        // Assert
        x.Should().Be(4);
        y.Should().Be(12);
    }

    [Fact]
    public void ManhattanDistance()
    {
        // Arrange
        var start = new Point(0, 0);
        var end = new Point(6, 6);

        // Act
        var result = start.ManhattanDistance(end);

        // Assert
        result.Should().Be(12);
    }
}