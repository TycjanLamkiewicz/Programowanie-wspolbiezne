//`Logic` uses only the abstract `Data` layer API
using Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Logic
{
    internal class Logic : LogicAPI
    {
        // DataAPI instance to handle data operations
        private DataAPI dataAPI;


        // Constructor (using DI)
        public Logic(DataAPI dataAPI)
        {
            this.dataAPI = dataAPI;
        }

        public override void CreateBalls(int num, int height, int width)
        {
            Random rnd = new Random();
            int radius = 25;    // All balls are assumed to be the same
            int mass = 5;
            int tableWidth = 700;
            int tableHeight = 400;

            // Generate balls with random positions and add them to the DataAPI
            for (int i = 0; i < num; i++) 
            {
                IBall ball = IBall.CreateBall(rnd.Next(radius, width - radius), rnd.Next(radius, height - radius), 2, 2, radius, mass, tableWidth, tableHeight);
                dataAPI.AddBall(ball);
                ball.PositionChange += HandlePositionChange;    // For each ball, the PositionChange event is subscribed to in order to react to changes in the ball's position.
            }
        }

        public override void RemoveBalls()
        {
            dataAPI.RemoveBalls();
        }

        public override int GetRadius()
        {
            return 25;
        }

        public override List<Vector2> GetPositions()
        {
            return dataAPI.GetPositions();
        }

        public override event EventHandler LogicEvent;      // Event (LogicEvent), which is triggered when the position of the ball changes 

        // In this method a check is made to ensure that the sender is different from null to avoid calling the event on the wrong object.
        // If the sender is not null, the LogicEvent event is called, passing the sender as the sender and EventArgs.Empty as the event argument.
        private void HandlePositionChange(object sender, EventArgs e)
        {
            if (sender != null)
            {
                LogicEvent?.Invoke(sender, EventArgs.Empty);
            }   
        }
    }
}
