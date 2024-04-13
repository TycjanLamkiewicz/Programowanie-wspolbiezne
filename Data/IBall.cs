using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IBall
    {
        public abstract float Position_x { get; }
        public abstract float Position_y { get;}
        public abstract float Speed_x { get; set; }
        public abstract float Speed_y { get; set; } 
        public abstract int Radius { get;}
        public abstract int Mass { get; }

        public event EventHandler PositionChange;

        public static IBall CreateBall(float position_x, float position_y, float speed_x, float speed_y, int radius, int mass) 
        {
            return new Ball(position_x, position_y, speed_x, speed_y, radius, mass);
        }
    }
}
