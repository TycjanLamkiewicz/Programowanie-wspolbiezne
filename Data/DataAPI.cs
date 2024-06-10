using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Data
{
    // `Data` API is clearly stated
    // `Data` API is abstract
    public abstract class DataAPI
    {
        // Abstract property to get a list of balls
        public abstract List<IBall>? Balls { get; }

        // Abstract method to add a ball
        public abstract void AddBall(IBall ball);

        // Abstract method to remove all balls
        public abstract void RemoveBalls();

        // Abstract method to get positions of all balls
        public abstract List<Vector2> GetPositions();

        // Abstract method to get position of a specific ball
        public abstract Vector2 GetPosition(IBall ball);

        public abstract event EventHandler<Tuple<Vector2, int, DateTime>> PositionEvent;

        // Static method to create an instance of DataAPI
        public static DataAPI CreateAPI()
        {
            return new Data();
        }
    }
}
