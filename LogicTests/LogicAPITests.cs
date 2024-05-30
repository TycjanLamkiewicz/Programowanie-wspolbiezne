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
                foreach (IBall ball in Balls)
                {
                    ball.StopTask();
                }
                balls.Clear();
            }

            public override List<Vector2> GetPositions()
            {
                List<Vector2> positions = new List<Vector2>();

                if (Balls != null)
                {
                    foreach (IBall ball in Balls)
                    {
                        positions.Add(ball.Position);
                    }
                }

                return positions;
            }

            public override Vector2 GetPosition(IBall ball)
            {
                return ball.Position;
            }
        }

        internal class Ball : IBall
        {
            // Private fields
            private Vector2 position;
            private Vector2 speed;
            private readonly int radius;
            private readonly int mass;
            private readonly int id;

            private bool is_running = true;
            private readonly int period = 5;
            private readonly object lock_move = new object();

            // Properties
            public Vector2 Position { get => position; private set => position = value; }
            public Vector2 Speed { get => speed; set => speed = value; }
            public int Radius { get => radius; }
            public int Mass { get => mass; }
            public int Id { get => id; }


            // Constructor
            public Ball(Vector2 position, Vector2 speed, int radius, int mass, int id)
            {
                this.position = position;
                this.speed = speed;
                this.radius = radius;
                this.mass = mass;
                this.id = id;
            }

            // Event triggered when the position of the ball changes
            public event EventHandler PositionChange;

            // This is a method that is responsible for calling the PositionChange event. 
            // It calls the event, passing itself(this) as the event sender and EventArgs.Empty as the event argument (as it does not require additional information).
            internal void OnPositionChange()
            {
                PositionChange?.Invoke(this, EventArgs.Empty);
            }
            public void StopTask()
            {
                is_running = false;
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
