using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Data
{
    internal class Ball : IBall
    {
        private float position_x;
        private float position_y;
        private float speed_x;
        private float speed_y;
        private int radius;
        private int mass;
        private Stopwatch stopwatch = new Stopwatch();
        private int period = 10;
        private Task? task;

        public float Position_x { get => position_x; private set => position_x = value; }
        public float Position_y { get => position_y; private set => position_y = value; }
        public float Speed_x { get => speed_x; set => speed_x = value; }
        public float Speed_y { get => speed_y; set => speed_y = value; }
        public int Radius { get => radius; }
        public int Mass { get => mass; }

        public Ball(float position_x, float position_y, float speed_x, float speed_y, int radius, int mass) 
        { 
            this.position_x = position_x;
            this.position_y = position_y;
            this.speed_x = speed_x;
            this.speed_y = speed_y;
            this.radius = radius;
            this.mass = mass;

            CreateTask();
        }

        public event EventHandler PositionChange;

        internal void OnPositionChange()
        {
            PositionChange?.Invoke(this, EventArgs.Empty);
        }

        private void Move()
        {
            Position_x += Speed_x;
            Position_y += Speed_y;
            OnPositionChange();
        }

        private void CreateTask()
        {
            task = Task.Run(async () =>
            {
                int waiting = 0;

                while (true)
                {
                    stopwatch.Restart();
                    stopwatch.Start();  
                    Move();
                    stopwatch.Stop();
                    if (period - stopwatch.ElapsedMilliseconds >= 0)
                    {
                        waiting = period - (int)stopwatch.ElapsedMilliseconds;
                    }
                    else
                    {
                        waiting = 0;
                    }

                    await Task.Delay(waiting);
                }
            });
        }

        public void KillTask()
        {
            task.Dispose();
        }
    }
}
