using System;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection;
using System.Threading;
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
        private int tableWidth;
        private int tableHeight;

        private readonly object lockMove = new object();
        private readonly object lockObject = new object();

        private Stopwatch stopwatch = new Stopwatch();
        private int period = 5;

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
            // Update ball position based on current speed
            lock (lockMove)
            {
                Position_x += Speed_x;
                Position_y += Speed_y;
            }
            
            // Trigger the PositionChange event to indicate that the ball's position has changed.
            OnPositionChange();
        }

        private void CreateTask()
        {
            Task task = Task.Run(async () =>
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
    }
}
