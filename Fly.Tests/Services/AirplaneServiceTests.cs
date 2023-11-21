namespace Fly.Tests.Services
{
    using System;
    using System.Net.Sockets;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Fly.Models.DTO;
    using Fly.Models.Entities;
    using Fly.Persistence;
    using Fly.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.ExtendedReflection.Collections;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Any;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AirplaneServiceTests
    {
        private IAirplaneService _testClass;
        private Mock<AirplaneDbContext> _context;
        private Mock<ILogger<AirplaneService>> _log;
        private Guid _id = Guid.Parse("23192332-639b-4817-9aab-20dfc686742c");
        private Mock<DbSet<Airplane>> _mockSet;

        public List<Airplane> GetAirplaneEntities()
        {
            var entities = new List<Airplane>
            {
                new Airplane { Id = _id, MaxSeats = 1, MaxVolumeCargo = 123, MaxWeightCargo = 321, Owner = "Sas", Type = "thing" }
            };
            return entities;
        }

      
        public AirplaneServiceTests()
        {
            _mockSet = new Mock<DbSet<Airplane>>();
            var data = GetAirplaneEntities().AsQueryable();

            _mockSet.As<IQueryable<Airplane>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<Airplane>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<Airplane>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<Airplane>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            _mockSet.Setup(set => set.FindAsync(It.IsAny<Guid>()).Result).Returns(GetAirplaneEntities()[0]);
;            _context = new Mock<AirplaneDbContext>();
            _context.Setup(m => m.Airplanes).Returns(_mockSet.Object);
            _log = new Mock<ILogger<AirplaneService>>();
            _testClass = new AirplaneService(_context.Object, _log.Object);
        }

        [TestMethod]
        public void CanConstruct()
        {
            // Act
            var instance = new AirplaneService(_context.Object, _log.Object);

            // Assert
            Assert.IsNotNull(instance);
        }


        [TestMethod]
        public async Task CanCallAddAirplaneAsync()
        {
            // Arrange
            var airplaneDto = new AirplaneDto
            {
                Type = "TestValue739973990",
                Owner = "TestValue1062162627",
                MaxWeightCargo = 1598740441.65,
                MaxVolumeCargo = 937718027.73,
                MaxSeats = 515344240
            };
            // Act
            var result = await _testClass.AddAirplaneAsync(airplaneDto);

            // Assert
            _context.Verify(m => m.Airplanes.Add(It.IsAny<Airplane>()));
            _context.Verify(m => m.SaveChanges(), Times.Once());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CanCallDeleteAirplaneAsync()
        {
            // Arrange
            var id = _id;

            // Act
            var result = await _testClass.DeleteAirplaneAsync(id);

            // Assert
            _context.Verify(m => m.Airplanes.FindAsync(It.IsAny<Guid>()), Times.Once());
            _context.Verify(m => m.Airplanes.Remove(It.IsAny<Airplane>()), Times.Once());
            _context.Verify(m => m.SaveChanges(), Times.Once());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CanCallGetAirplaneByIdAsync()
        {
            // Arrange
            var id = _id;

            // Act
            var expectedJson = JsonSerializer.Serialize( GetAirplaneEntities()[0]);
            var resultJson = JsonSerializer.Serialize(await _testClass.GetAirplaneByIdAsync(id));

            // Assert
            _context.Verify(m => m.Airplanes.FindAsync(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(expectedJson,resultJson); 
        }

        [TestMethod]
        public async Task GetAirplaneByIdAsyncPerformsMapping()
        {
            // Arrange
            var id = _id;

            // Act
            var result = await _testClass.GetAirplaneByIdAsync(id);

            // Assert
            _context.Verify(m => m.Airplanes.FindAsync(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        public async Task CanCallGetAllAirplaneAsync()
        {
            // Act
            var result = await _testClass.GetAllAirplaneAsync();

            // Assert
            Assert.AreEqual(GetAirplaneEntities().Count, result.Count);
        }
    }
}