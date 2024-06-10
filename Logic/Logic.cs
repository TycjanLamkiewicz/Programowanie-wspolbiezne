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
            int id = 0;

            // Generate balls with random positions and add them to the DataAPI
            for (int i = 0; i < num; i++) 
            {
                id++;
                IBall ball = IBall.CreateBall(new Vector2(rnd.Next(25, width - 25), rnd.Next(25, height - 25)), new Vector2(0.25f,0.25f), id);
                dataAPI.AddBall(ball);
                ball.PositionChange += HandlePositionChange;    // For each ball, the PositionChange event is subscribed to in order to react to changes in the ball's position.
            }
        }

        public override void RemoveBalls()
        {
            dataAPI.RemoveBalls();
        }

        public override List<Vector2> GetPositions()
        {
            return dataAPI.GetPositions();
        }

        public override event EventHandler<Tuple<Vector2, int, DateTime>> LogicEvent;      // Event (LogicEvent), which is triggered when the position of the ball changes 

        private readonly object lockColision = new object();

        // In this method a check is made to ensure that the sender is different from null to avoid calling the event on the wrong object.
        // If the sender is not null, the LogicEvent event is called, passing the sender as the sender and EventArgs.Empty as the event argument.
        private void HandlePositionChange(object sender, Tuple<Vector2, int, DateTime> e)
        {
            // Cast the 'sender' object to an IBall interface to work with ball-specific properties
            IBall ball = (IBall)sender;
            
            lock (lockColision)
            {
                CheckCollisionWithWalls(ball);
                CheckCollisionWithBalls(ball);

                // Invoke the LogicEvent to signal any logic updates related to the ball's movement
                LogicEvent?.Invoke(sender, e);
            }  
        }

        private readonly int tableWidth = 700;
        private readonly int tableHeight = 400;

        private void CheckCollisionWithWalls(IBall ball)
        {
            // Create a new vector to store the updated speed of the ball
            Vector2 newSpeed = ball.Speed;

            // Ensure the ball stays within the x-axis bounds of the table
            if (ball.Position.X <= 0)
            {
                newSpeed.X = Math.Abs(ball.Speed.X);
            }

            if (ball.Position.X + 25 >= tableWidth)
            {
                newSpeed.X = -Math.Abs(ball.Speed.X);
            }

            // Ensure the ball stays within the y-axis bounds of the table
            if (ball.Position.Y <= 0)
            {
                newSpeed.Y = Math.Abs(ball.Speed.Y);
            }
            if (ball.Position.Y + 25 >= tableHeight)
            {
                newSpeed.Y = -Math.Abs(ball.Speed.Y);
            }

            // Update the ball's speed with the new values
            ball.Speed = newSpeed;
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
                        float dx = ball.Position.X - ball2.Position.X;
                        float dy = ball.Position.Y - ball2.Position.Y;
                        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                        // Check if the collision is close to the wall
                        bool nearWall = ball.Position.X - 25 <= 0 || ball.Position.X + 25 >= tableWidth ||
                                        ball.Position.Y - 25 <= 0 || ball.Position.Y + 25 >= tableHeight;

                        // Check if a collision occurs and it's not near the wall
                        if (distance - 25 <= 0 && !nearWall)
                        {
                            float speed1_x, speed1_y, speed2_x, speed2_y;

                            // Calculate the new velocities of both balls after the collision
                            speed1_x = (5 * ball.Speed.X + 5 * ball2.Speed.X - 5 * (ball.Speed.X - ball2.Speed.X)) / (5 + 5);
                            speed1_y = (5 * ball.Speed.Y + 5 * ball2.Speed.Y - 5 * (ball.Speed.Y - ball2.Speed.Y)) / (5 + 5);
                            speed2_x = (5 * ball.Speed.X + 5 * ball2.Speed.X - 5 * (ball2.Speed.X - ball.Speed.X)) / (5 + 5);
                            speed2_y = (5 * ball.Speed.Y + 5 * ball2.Speed.Y - 5 * (ball2.Speed.Y - ball.Speed.Y)) / (5 + 5);

                            // Update the speeds of both balls
                            ball.Speed = new Vector2(speed1_x, speed1_y);
                            ball2.Speed = new Vector2(speed2_x, speed2_y);

                            lastCollision[ball] = DateTime.Now;
                            lastCollision[ball2] = DateTime.Now;
                        }
                    }
                }
            }
        }
    }
}
