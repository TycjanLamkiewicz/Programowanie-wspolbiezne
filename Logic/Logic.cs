//`Logic` uses only the abstract `Data` layer API
using Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Logic
{
    internal class Logic : LogicAPI
    {
        // DataAPI instance to handle data operations
        private DataAPI dataAPI;


        // Constructor (using DI)
        public Logic(DataAPI dataAPI)
        {
            this.dataAPI = dataAPI;
        }

        private Dictionary<IBall, DateTime> lastCollision = new Dictionary<IBall, DateTime>();

        public override void CreateBalls(int num, int height, int width)
        {
            Random rnd = new Random();
            int radius = 25;    // All balls are assumed to be the same
            int mass = 5;
            int tableWidth = 700;
            int tableHeight = 400;

            // Generate balls with random positions and add them to the DataAPI
            for (int i = 0; i < num; i++) 
            {
                IBall ball = IBall.CreateBall(rnd.Next(radius, width - radius), rnd.Next(radius, height - radius), 2, 2, radius, mass, tableWidth, tableHeight);
                dataAPI.AddBall(ball);
                ball.PositionChange += HandlePositionChange;    // For each ball, the PositionChange event is subscribed to in order to react to changes in the ball's position.
            }
        }

        public override void RemoveBalls()
        {
            dataAPI.RemoveBalls();
        }

        public override int GetRadius()
        {
            return 25;
        }

        public override List<Vector2> GetPositions()
        {
            return dataAPI.GetPositions();
        }

        public override event EventHandler LogicEvent;      // Event (LogicEvent), which is triggered when the position of the ball changes 

        private readonly object lockColision = new object();

        // In this method a check is made to ensure that the sender is different from null to avoid calling the event on the wrong object.
        // If the sender is not null, the LogicEvent event is called, passing the sender as the sender and EventArgs.Empty as the event argument.
        private void HandlePositionChange(object sender, EventArgs e)
        {
            // Cast the 'sender' object to an IBall interface to work with ball-specific properties
            IBall ball = (IBall)sender;

            // Check if the sender object is not null
            if (sender != null)
            {
                lock (lockColision)
                {
                    CheckCollisionWithWalls(ball);
                    CheckCollisionWithBalls(ball);
                }
                // Invoke the LogicEvent to signal any logic updates related to the ball's movement
                LogicEvent?.Invoke(sender, EventArgs.Empty);

            }   
        }

        private void CheckCollisionWithWalls(IBall ball)
        {
            // Create a new vector to store the updated speed of the ball
            Vector2 newSpeed = new Vector2(ball.Speed_x, ball.Speed_y);

            // Ensure the ball stays within the x-axis bounds of the table
            if (ball.Position_x < 0 || ball.Position_x + ball.Radius >= ball.TableWidth)
            {
                newSpeed.X = ball.Speed_x *= -1;       // Reverse the x-direction speed
            }

            // Ensure the ball stays within the y-axis bounds of the table
            if (ball.Position_y < 0 || ball.Position_y + ball.Radius >= ball.TableHeight)
            {
                newSpeed.Y = ball.Speed_y *= -1;       // Reverse the y-direction speed
            }

            // Update the ball's speed with the new values
            ball.Speed_x = newSpeed.X;
            ball.Speed_y = newSpeed.Y;
        }

        // This method checks for collisions between the given ball and other balls on the table.
        // If a collision is detected and it's not too close to the wall, it calculates and updates
        // the speeds of both balls based on their masses and velocities.
        private void CheckCollisionWithBalls(IBall ball)
        {
            foreach (IBall ball2 in dataAPI.Balls)
            {
                if (ball != ball2) // Avoid self-collision
                {
                    if (!lastCollision.ContainsKey(ball) || (DateTime.Now - lastCollision[ball]).TotalMilliseconds > 100)
                    {
                        // Calculate the distance between the centers of the two balls
                        float dx = ball.Position_x - ball2.Position_x;
                        float dy = ball.Position_y - ball2.Position_y;
                        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                        // Check if the collision is close to the wall
                        bool nearWall = ball.Position_x - ball.Radius <= 0 || ball.Position_x + ball.Radius >= ball.TableWidth ||
                                        ball.Position_y - ball.Radius <= 0 || ball.Position_y + ball.Radius >= ball.TableHeight;

                        // Check if a collision occurs and it's not near the wall
                        if (distance - ball.Radius <= 0 && !nearWall)
                        {
                            float speed1_x, speed1_y, speed2_x, speed2_y;

                            // Calculate the new velocities of both balls after the collision
                            speed1_x = (ball.Mass * ball.Speed_x + ball2.Mass * ball2.Speed_x - ball2.Mass * (ball.Speed_x - ball2.Speed_x)) / (ball.Mass + ball2.Mass);
                            speed1_y = (ball.Mass * ball.Speed_y + ball2.Mass * ball2.Speed_y - ball2.Mass * (ball.Speed_y - ball2.Speed_y)) / (ball.Mass + ball2.Mass);
                            speed2_x = (ball.Mass * ball.Speed_x + ball2.Mass * ball2.Speed_x - ball.Mass * (ball2.Speed_x - ball.Speed_x)) / (ball.Mass + ball2.Mass);
                            speed2_y = (ball.Mass * ball.Speed_y + ball2.Mass * ball2.Speed_y - ball2.Mass * (ball2.Speed_y - ball.Speed_y)) / (ball.Mass + ball2.Mass);

                            // Update the speeds of both balls
                            ball.Speed_x = speed1_x;
                            ball.Speed_y = speed1_y;
                            ball2.Speed_x = speed2_x;
                            ball2.Speed_y = speed2_y;

                            lastCollision[ball] = DateTime.Now;
                            lastCollision[ball2] = DateTime.Now;
                        }
                    }
                }
            }
        }
    }
}
