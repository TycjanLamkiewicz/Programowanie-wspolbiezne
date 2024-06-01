using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Text;

namespace Model
{
    public abstract class IBallModel
    {
        // Abstract properties for the position (x and y) and the radius of the ball
        public abstract float Position_x { get; }
        public abstract float Position_y { get; }
        public abstract int Radius { get; }
        public abstract int Id { get; }

        public abstract void setPosition(Vector2 position);

        // Static method to create an instance of IBallModel
        public static IBallModel CreateBallModel(int radius, int id)
        {
            return new BallModel(radius, id);
        }
    }
}
