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
        private DataAPI dataAPI;


        public Logic(DataAPI dataAPI)
        {
            this.dataAPI = dataAPI;
        }

        public override void CreateBalls(int num, int height, int width)
        {
            Random rnd = new Random();
            int radius = 25;
            int mass = 5;

            for (int i = 0; i < num; i++) 
            {
                IBall ball = IBall.CreateBall(rnd.Next(radius, width - radius), rnd.Next(radius, height - radius), 2, 2, radius, mass);
                dataAPI.AddBall(ball);
                ball.PositionChange += HandlePositionChange;
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

        public override event EventHandler LogicEvent;

        private void HandlePositionChange(object sender, EventArgs e)
        {
            if (sender != null)
            {
                LogicEvent?.Invoke(sender, EventArgs.Empty);
            }   
        }
    }
}
