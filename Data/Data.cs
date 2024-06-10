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
        // List to store balls
        private List<IBall> balls = new List<IBall>();

        public override List<IBall>? Balls { get => balls; }

        private Logger logger;
        public override event EventHandler<Tuple<Vector2, int, DateTime>> PositionEvent;

        public Data()
        {
            logger = new Logger();
        }

        private void PositionChanged(object sender, Tuple<Vector2,int, DateTime> e)
        {
            PositionEvent?.Invoke(sender, e);
            logger.Add((IBall)sender, e.Item3);
        }

        public override void AddBall(IBall ball)
        {
            balls.Add(ball);
            ball.PositionChange += PositionChanged;
        }   

        public override void RemoveBalls()
        {
            foreach (IBall ball in Balls)
            {
                ball.StopTask();
            }
            balls.Clear();
        }

        public override List<Vector2> GetPositions()
        {
            List<Vector2> positions = new List<Vector2>();

            if (Balls != null)
            {
                foreach (IBall ball in Balls)
                {
                    positions.Add(ball.Position);
                }
            }
           
            return positions;
        }

        public override Vector2 GetPosition(IBall ball)
        {
            return ball.Position;
        }
    }
}
