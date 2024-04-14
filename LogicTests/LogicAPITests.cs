using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Data;
using Logic;
using Xunit;

namespace LogicTests
{
    public class LogicAPITests
    {
            public class DataTest : DataAPI
            {
            // List to store balls
            private List<IBall> balls = new List<IBall>();


            public override List<IBall>? Balls { get => balls; }

            public override void AddBall(IBall ball)
            {
                balls.Add(ball);
            }

            public override void RemoveBalls()
            {
                balls.Clear();
            }

            public override List<Vector2> GetPositions()
            {
                List<Vector2> positions = new List<Vector2>();

                if (Balls != null)
                {
                    foreach (IBall ball in Balls)
                    {
                        positions.Add(new Vector2(ball.Position_x, ball.Position_y));
                    }
                }

                return positions;
            }

            public override Vector2 GetPosition(IBall ball)
            {
                return new Vector2(ball.Position_x, ball.Position_y);
            }
        }
        // Test for creating balls
        [Fact]
        public void CreateBallsTest()
        {
            // Arrange
            int numBalls = 5;
            int tableWidth = 700;
            int tableHeight = 400;
            LogicAPI logicAPI = LogicAPI.CreateAPI(new DataTest());

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
            LogicAPI logicAPI = LogicAPI.CreateAPI(new DataTest());
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
            LogicAPI logicAPI = LogicAPI.CreateAPI(new DataTest());

            // Act
            int radius = logicAPI.GetRadius();

            // Assert
            Assert.Equal(25, radius);
        }
    }
}
