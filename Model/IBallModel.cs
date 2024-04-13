using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model
{
    public abstract class IBallModel
    {
        public abstract float Position_x { get; set; }
        public abstract float Position_y { get; set; }
        public abstract int Radius { get; }

        public static IBallModel CreateBallModel(float position_x, float position_y, int radius)
        {
            return new BallModel(position_x, position_y, radius);    
        }
    }
}
