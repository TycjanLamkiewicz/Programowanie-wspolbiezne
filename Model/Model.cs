using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Model
{
    internal class Model : ModelAPI
    {
        private LogicAPI logicAPI;
        private ObservableCollection<IBallModel> ballsModel = new ObservableCollection<IBallModel>();
        public override event EventHandler ModelEvent;
        private int num;

        public Model(LogicAPI logicAPI)
        {
            this.logicAPI = logicAPI;
            logicAPI.LogicEvent += ChangeModelBallsPositions;
        }

        public override void CreateBalls(int num, int height, int width)
        {
            this.num = num;
            logicAPI.CreateBalls(num, height, width);
        }

        public override void RemoveAllBalls()
        {
            logicAPI.RemoveBalls();
        }

        public override ObservableCollection<IBallModel> GetBallsModel()
        {
            ballsModel.Clear();
            int radius = logicAPI.GetRadius();

            foreach (Vector2 ball in logicAPI.GetPositions())
            {
                IBallModel ballModel = IBallModel.CreateBallModel(ball.X, ball.Y, radius);
                ballsModel.Add(ballModel);
            }

            return ballsModel;
        }

        private void ChangeModelBallsPositions(object sender, EventArgs e)
        {
            int i = 0;

            foreach (Vector2 ball in logicAPI.GetPositions())
            {
                if (num == ballsModel.Count)
                {
                    ballsModel[i].Position_x = ball.X;
                    ballsModel[i].Position_y = ball.Y;
                    i++;
                }
            }
            
        }
    }
}
