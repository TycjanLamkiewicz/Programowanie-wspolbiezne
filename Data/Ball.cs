using System;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection;
using System.Threading.Tasks;

namespace Data
{
    internal class Ball : IBall
    {
        // Private fields
        private float position_x;
        private float position_y;
        private float speed_x;
        private float speed_y;
        private int radius;
        private int mass;
        private Stopwatch stopwatch = new Stopwatch();
        private int period = 4;
        private Task? task;

        // Properties
        public float Position_x { get => position_x; private set => position_x = value; }
        public float Position_y { get => position_y; private set => position_y = value; }
        public float Speed_x { get => speed_x; set => speed_x = value; }
        public float Speed_y { get => speed_y; set => speed_y = value; }
        public int Radius { get => radius; }
        public int Mass { get => mass; }
        public int TableWidth { get; set; }
        public int TableHeight { get; set; }

        // Constructor
        public Ball(float position_x, float position_y, float speed_x, float speed_y, int radius, int mass, int tableWidth, int tableHeight) 
        { 
            this.position_x = position_x;
            this.position_y = position_y;
            this.speed_x = speed_x;
            this.speed_y = speed_y;
            this.radius = radius;
            this.mass = mass;
            this.TableWidth = tableWidth;
            this.TableHeight = tableHeight;

            CreateTask();   // Create a task to move the ball
        }

        // Event triggered when the position of the ball changes
        public event EventHandler PositionChange;
        
        // This is a method that is responsible for calling the PositionChange event. 
        // It calls the event, passing itself(this) as the event sender and EventArgs.Empty as the event argument (as it does not require additional information).
        internal void OnPositionChange()
        {
            PositionChange?.Invoke(this, EventArgs.Empty);
        }

        private void Move()
        {
            // Generate random speeds for x and y directions
            Random rnd = new Random();
            Speed_x = (float)(rnd.NextDouble() * 2 - 1);
            Speed_y = (float)(rnd.NextDouble() * 2 - 1);

            // Update ball position based on current speed
            Position_x += Speed_x;
            Position_y += Speed_y;

            // Ensure the ball stays within the boundaries of the table
            EnsureWithinTableBounds();

            // Trigger the PositionChange event to indicate that the ball's position has changed.
            OnPositionChange();
        }

        private void EnsureWithinTableBounds()
        {
            // Ensure the ball stays within the x-axis bounds of the table
            if (Position_x < Radius)
            {
                Position_x = Radius; // Move the ball back within the bounds
                Speed_x *= -1;       // Reverse the x-direction speed
            }
            else if (Position_x > TableWidth - Radius)
            {
                Position_x = TableWidth - Radius; // Move the ball back within the bounds
                Speed_x *= -1;                    // Reverse the x-direction speed
            }

            // Ensure the ball stays within the y-axis bounds of the table
            if (Position_y < Radius)
            {
                Position_y = Radius; // Move the ball back within the bounds
                Speed_y *= -1;       // Reverse the y-direction speed
            }
            else if (Position_y > TableHeight - Radius)
            {
                Position_y = TableHeight - Radius; // Move the ball back within the bounds
                Speed_y *= -1;                     // Reverse the y-direction speed
            }
        }

        private void CreateTask()
        {
            task = Task.Run(async () =>
            {
                int waiting = 0;            // This variable will store the waiting time before the next ball movement is made.

                while (true)
                {
                    stopwatch.Restart();    // Restart the stopwatch to measure time
                    stopwatch.Start();      // Start the stopwatch
                    Move();
                    stopwatch.Stop();       // Stop the stopwatch

                    // This conditional instruction calculates the waiting time before the next ball movement is made.
                    // It subtracts the duration of the ball movement from the expected period between movements.
                    // If the result is greater than 0, the ball will wait for this time.
                    // Otherwise,if the duration of the ball's movement is longer than the expected period, the waiting variable will be equal to 0,
                    // meaning that the ball will continue moving immediately.
                    if (period - stopwatch.ElapsedMilliseconds > 0)
                    {
                        waiting = period - (int)stopwatch.ElapsedMilliseconds;
                    }
                    else
                    {
                        waiting = 0;
                    }

                    // The task waits for a specified amount of time(wait) before the next ball move is executed.
                    // await causes the task to be suspended without blocking the thread, allowing other operations to use the processor.
                    // When the wait time expires, the task resumes and the cycle starts again, with the next ball move executed.
                    await Task.Delay(waiting);
                }
            });
        }

        // Method to dispose the task when needed
        public void KillTask()
        {
            task.Dispose();
        }
    }
}
