using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Data
{
    public interface IBall
    {
        // Properties
        public abstract Vector2 Position { get; }
        public abstract Vector2 Speed { get; set; }
        public abstract int Id { get; }
        public void StopTask();

        // Event triggered when the position of the ball changes
        public event EventHandler PositionChange;
        
        // Method to create a new ball instance with specified parameters
        public static IBall CreateBall(Vector2 position, Vector2 speed, int id) 
        {
            return new Ball(position, speed, id);
        }
    }
}
