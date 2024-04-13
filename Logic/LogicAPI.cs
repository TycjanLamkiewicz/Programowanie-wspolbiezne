using System;
using System.Collections.Generic;
using System.Numerics;
using Data;

namespace Logic
{
    public abstract class LogicAPI
    {
        public abstract void CreateBalls(int num, int height, int width);
        public abstract void RemoveBalls();
        public abstract int GetRadius();
        public abstract List<Vector2> GetPositions();
        public abstract event EventHandler LogicEvent;

        public static LogicAPI CreateAPI()
        {
            return new Logic(DataAPI.CreateAPI());
        }
    }
}
