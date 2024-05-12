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

        internal class Ball : IBall
        {
            // Private fields
            private float position_x;
            private float position_y;
            private float speed_x;
            private float speed_y;
            private int radius;
            private int mass;
            private int tableWidth;
            private int tableHeight;

            private readonly object lockObject = new object();

            // Properties
            public float Position_x
            {
                get { lock (lockObject) { return position_x; } }
                private set { lock (lockObject) { position_x = value; } }
            }

            public float Position_y
            {
                get { lock (lockObject) { return position_y; } }
                private set { lock (lockObject) { position_y = value; } }
            }

            public float Speed_x
            {
                get { lock (lockObject) { return speed_x; } }
                set { lock (lockObject) { speed_x = value; } }
            }

            public float Speed_y
            {
                get { lock (lockObject) { return speed_y; } }
                set { lock (lockObject) { speed_y = value; } }
            }

            public int Radius
            {
                get { lock (lockObject) { return radius; } }
            }

            public int Mass
            {
                get { lock (lockObject) { return mass; } }
            }

            public int TableWidth
            {
                get { lock (lockObject) { return tableWidth; } }
                set { lock (lockObject) { tableWidth = value; } }
            }

            public int TableHeight
            {
                get { lock (lockObject) { return tableHeight; } }
                set { lock (lockObject) { tableHeight = value; } }
            }

            // Constructor
            public Ball(float position_x, float position_y, float speed_x, float speed_y, int radius, int mass, int tableWidth, int tableHeight)
            {
                this.position_x = position_x;
                this.position_y = position_y;
                this.speed_x = speed_x;
                this.speed_y = speed_y;
                this.radius = radius;
                this.mass = mass;
                this.tableWidth = tableWidth;
                this.tableHeight = tableHeight;
            }

            // Event triggered when the position of the ball changes
            public event EventHandler PositionChange;
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
