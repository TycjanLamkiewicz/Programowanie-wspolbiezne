using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Data
{
    public abstract class DataAPI
    {
        public abstract List<IBall>? Balls { get; }
        public abstract void AddBall(IBall ball);
        public abstract void RemoveBalls();
        public abstract List<Vector2> GetPositions();
        public abstract Vector2 GetPosition(IBall ball);

        public static DataAPI CreateAPI()
        {
            return new Data();
        }
    }
}
