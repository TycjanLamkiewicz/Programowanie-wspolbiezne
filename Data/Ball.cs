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
        private readonly int id;

        private bool is_running = true;
        private readonly int period = 4;
        private readonly object lock_position = new object();
        private readonly object lock_speed = new object();

        // Properties
        public Vector2 Position 
        { 
            get
            {
                lock (lock_position)
                {
                    return position;
                }
            }
            private set
            {
                lock(lock_position)
                {
                    position = value;
                }
                OnPositionChange(DateTime.UtcNow);
            }
        }
        public Vector2 Speed 
        {
            get
            {
                lock(lock_speed)
                {
                    return speed;
                }
            }
            set
            {
                lock (lock_speed)
                {
                    speed = value;
                }
            }
        }
        public int Id { get => id; }

        // Constructor
        public Ball(Vector2 position, Vector2 speed, int id) 
        { 
            this.position = position;
            this.speed = speed;
            this.id = id;

            CreateTask();   // Create a task to move the ball
        }

        // Event triggered when the position of the ball changes
        public event EventHandler<Tuple<Vector2, int, DateTime>> PositionChange;
        
        // This is a method that is responsible for calling the PositionChange event. 
        // It calls the event, passing itself(this) as the event sender and EventArgs.Empty as the event argument (as it does not require additional information).
        internal void OnPositionChange(DateTime time)
        {
            PositionChange?.Invoke(this, new Tuple<Vector2, int, DateTime>(position, id, time));
        }

        private void Move(int elapsed_time)
        {
            // Update ball position based on current speed
            Position += Speed * elapsed_time;
        }

        private void CreateTask()
        {
            Task task = Task.Run(async () =>
            {
                Stopwatch stopwatch = new Stopwatch();
                int waiting = 0;            // This variable will store the waiting time before the next ball movement is made.
                int last_move = 0;

                stopwatch.Start();

                while (is_running)
                {
                    int current_time = (int)stopwatch.ElapsedMilliseconds;
                    int elapsed_time = current_time - last_move;
                    
                    Move(elapsed_time);

                    last_move = current_time;

                    if (period - elapsed_time > 0)
                    {
                        waiting = period - elapsed_time;
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
