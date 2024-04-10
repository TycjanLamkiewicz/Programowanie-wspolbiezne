using System;

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

        public override float Position_x { get => position_x; }
        public override float Position_y { get => position_y; }
        public override float Speed_x { get => speed_x; }
        public override float Speed_y { get => speed_y; }
        public override int Radius { get => radius; }
        public override int Mass { get => mass; }

        public Ball(float position_x, float position_y, float speed_x, float speed_y, int radius, int mass) 
        { 
            this.position_x = position_x;
            this.position_y = position_y;
            this.speed_x = speed_x;
            this.speed_y = speed_y;
            this.radius = 50;
            this.mass = 50;
        }
    }
}
