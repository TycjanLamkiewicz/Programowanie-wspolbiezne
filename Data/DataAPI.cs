using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    internal abstract class DataAPI
    {
        public abstract List<IBall>? Balls { get; }
        public abstract void AddBall(IBall ball);
        public abstract void RemoveBall(IBall ball);

        public static DataAPI CreateAPI()
        {
            return new Data();
        }
    }
}
