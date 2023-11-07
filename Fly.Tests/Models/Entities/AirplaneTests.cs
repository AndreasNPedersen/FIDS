namespace Fly.Tests.Models.Entities
{
    using System;
    using Fly.Models.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AirplaneTests
    {
        private Airplane _testClass;

        [TestInitialize]
        public void SetUp()
        {
            _testClass = new Airplane();
        }

        [TestMethod]
        public void CanSetAndGetId()
        {
            // Arrange
            var testValue = new Guid("c4cd8701-ec3e-48ad-be77-85964d240035");

            // Act
            _testClass.Id = testValue;

            // Assert
            Assert.AreEqual(testValue, _testClass.Id);
        }

        [TestMethod]
        public void CanSetAndGetType()
        {
            // Arrange
            var testValue = "TestValue2123073656";

            // Act
            _testClass.Type = testValue;

            // Assert
            Assert.AreEqual(testValue, _testClass.Type);
        }

        [TestMethod]
        public void CanSetAndGetOwner()
        {
            // Arrange
            var testValue = "TestValue12024240";

            // Act
            _testClass.Owner = testValue;

            // Assert
            Assert.AreEqual(testValue, _testClass.Owner);
        }

        [TestMethod]
        public void CanSetAndGetMaxWeightCargo()
        {
            // Arrange
            var testValue = 358005927.51;

            // Act
            _testClass.MaxWeightCargo = testValue;

            // Assert
            Assert.AreEqual(testValue, _testClass.MaxWeightCargo);
        }

        [TestMethod]
        public void CanSetAndGetMaxVolumeCargo()
        {
            // Arrange
            var testValue = 1912922534.16;

            // Act
            _testClass.MaxVolumeCargo = testValue;

            // Assert
            Assert.AreEqual(testValue, _testClass.MaxVolumeCargo);
        }

        [TestMethod]
        public void CanSetAndGetMaxSeats()
        {
            // Arrange
            var testValue = 1467594513;

            // Act
            _testClass.MaxSeats = testValue;

            // Assert
            Assert.AreEqual(testValue, _testClass.MaxSeats);
        }
    }
}