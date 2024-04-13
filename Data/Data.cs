using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Data
{
    internal class Data : DataAPI
    {
        private List<IBall> balls = new List<IBall>();


        public override List<IBall>? Balls { get => balls; }

        public override void AddBall(IBall ball)
        {
            balls.Add(ball);
        }   

        public override void RemoveBalls()
        {
            balls.Clear();
        }

        public override List<Vector2> GetPositions()
        {
            List<Vector2> positions = new List<Vector2>();

            if (Balls != null)
            {
                foreach (IBall ball in Balls)
                {
                    positions.Add(new Vector2(ball.Position_x, ball.Position_y));
                }
            }
           
            return positions;
        }

        public override Vector2 GetPosition(IBall ball)
        {
            return new Vector2(ball.Position_x, ball.Position_y);
        }
    }
}
