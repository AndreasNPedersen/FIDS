using FIDS.Backend.Controllers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FIDS.Backend.Tests;

public class UnitTest1
{
    [Fact]
    public void TestArrivalsController_GetAllArrivals()
    {
        // Arrange
        var controller = new ArrivalsController(Substitute.For<ILogger<DeparturesController>>());

        // Act
        var result = controller.GetArrivals();

        // Assert
        result.Result.ToList().Count.Should().Be(1);
    }
}