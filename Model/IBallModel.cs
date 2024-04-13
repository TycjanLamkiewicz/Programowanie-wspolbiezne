using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model
{
    public abstract class IBallModel
    {
        // Abstract properties for the position (x and y) and the radius of the ball
        public abstract float Position_x { get; set; }
        public abstract float Position_y { get; set; }
        public abstract int Radius { get; }

        // Static method to create an instance of IBallModel
        public static IBallModel CreateBallModel(float position_x, float position_y, int radius)
        {
            return new BallModel(position_x, position_y, radius);
        }
    }
}
