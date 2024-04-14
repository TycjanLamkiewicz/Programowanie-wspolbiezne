using System;
using System.Collections.Generic;
using System.Numerics;
//`Logic` uses only the abstract `Data` layer API
using Data;

namespace Logic
{
    // `Logic` API is clearly stated
    // `Logic` uses only the abstract `Data` layer API
    public abstract class LogicAPI
    {
        // Abstract method to create balls
        public abstract void CreateBalls(int num, int height, int width);

        // Abstract method to remove all balls
        public abstract void RemoveBalls();

        // Abstract method to get the radius of balls
        public abstract int GetRadius();

        // Abstract method to get positions of all balls
        public abstract List<Vector2> GetPositions();

        // Abstract event triggered by logic operations
        public abstract event EventHandler LogicEvent;

        // Method to create an instance of LogicAPI (using DI)
        public static LogicAPI CreateAPI()
        {
            return new Logic(DataAPI.CreateAPI());
        }

        public static LogicAPI CreateAPI(DataAPI dataAPI)
        {
            return new Logic(dataAPI);
        }
    }
}
