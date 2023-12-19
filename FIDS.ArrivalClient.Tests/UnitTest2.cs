namespace FIDS.ArrivalClient.Tests;
using FluentAssertions;
using NSubstitute;
public class UnitTest2
{
    [Fact]
    public void TestArrivalsController_GetAllArrivals()
    {
        // Arrange
        var flyTest = new FlyTest();

        // Act
        var result = flyTest.ValidateFliteId(1);

        // Assert
        result.Should().Be(1);
    }
}
public  class FlyTest
{
    public  int ValidateFliteId(int id)
    {

        return 2;
    }
}
