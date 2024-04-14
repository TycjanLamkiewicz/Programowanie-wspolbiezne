using System;
using System.Collections.Generic;
using System.Numerics;
using Logic;
using Xunit;

namespace LogicTests
{
    public class LogicAPITests
    {
        // Test for creating balls
        [Fact]
        public void CreateBallsTest()
        {
            // Arrange
            int numBalls = 5;
            int tableWidth = 700;
            int tableHeight = 400;
            LogicAPI logicAPI = LogicAPI.CreateAPI();

            // Act
            logicAPI.CreateBalls(numBalls, tableHeight, tableWidth);
            List<Vector2> positions = logicAPI.GetPositions();

            // Assert
            Assert.Equal(numBalls, positions.Count);
        }

        // Test for removing all balls
        [Fact]
        public void RemoveBallsTest()
        {
            // Arrange
            int numBalls = 5;
            int tableWidth = 700;
            int tableHeight = 400;
            LogicAPI logicAPI = LogicAPI.CreateAPI();
            logicAPI.CreateBalls(numBalls, tableHeight, tableWidth);

            // Act
            logicAPI.RemoveBalls();
            List<Vector2> positions = logicAPI.GetPositions();

            // Assert
            Assert.Empty(positions);
        }

        // Test for getting the radius of balls
        [Fact]
        public void GetRadiusTest()
        {
            // Arrange
            LogicAPI logicAPI = LogicAPI.CreateAPI();

            // Act
            int radius = logicAPI.GetRadius();

            // Assert
            Assert.Equal(25, radius);
        }
    }
}
