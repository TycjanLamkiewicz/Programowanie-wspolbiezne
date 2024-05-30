using System;
using System.Diagnostics;
using System.Dynamic;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
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
        public Vector2 Position { 
            get
            {
                lock (lock_move)
                {
                    return position;
                }
            }
            private set => position = value; }
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
            lock(lock_move)
            {
                // Update ball position based on current speed
                Position += Speed * 1;
            }
            // Trigger the PositionChange event to indicate that the ball's position has changed.
            OnPositionChange();
        }

        private void CreateTask()
        {
            Task task = Task.Run(async () =>
            {
                Stopwatch stopwatch = new Stopwatch();
                int waiting = 0;            // This variable will store the waiting time before the next ball movement is made.

                while (is_running)
                {
                    stopwatch.Restart();    // Restart the stopwatch to measure time
                    stopwatch.Start();      // Start the stopwatch
                    // Do move musi byc przekazany czas
                    // Blad ze stopwatchem zeby nnie byl calkowany (dodawany za kazdym razem)
                    // Po restarcie jest on dodawany 
                    // Trzeba wziac roznice??
                    // nie wolno robic restartu, tylko od tego obliczyc roznice??
                    // 
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

        public void StopTask()
        {
            is_running = false;
        }
    }
}
