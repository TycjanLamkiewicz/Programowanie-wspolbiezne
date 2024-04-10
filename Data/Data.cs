using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    internal class Data : DataAPI
    {
        private List<IBall>? balls;
        public override List<IBall>? Balls { get => balls; }

        public override void AddBall(IBall ball)
        {
            balls.Add(ball);
        }

        public override void RemoveBall(IBall ball)
        {
            balls.Remove(ball);
        }
    }
}
