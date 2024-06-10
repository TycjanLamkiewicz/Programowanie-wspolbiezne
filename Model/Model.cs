// `Presentation` uses only the abstract `Logic` layer API
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
        // Collection to store ball models
        private ObservableCollection<IBallModel> ballsModel = new ObservableCollection<IBallModel>();
        // Event for model change notification
        public override event EventHandler ModelEvent;
        // Variable to store the number of balls

        // Constructor (using DI)
        public Model(LogicAPI logicAPI)
        {
            this.logicAPI = logicAPI;
            logicAPI.LogicEvent += ChangeModelBallsPositions;
        }

        // Method to create balls in the model
        public override void CreateBalls(int num, int height, int width)
        {
            logicAPI.CreateBalls(num, height, width);
            for (int i = 1; i <= num; i++) 
            { 
                IBallModel ball = new BallModel(25, i);
                ballsModel.Add(ball);
            }
        }

        // Method to remove all balls from the model
        public override void RemoveAllBalls()
        {
            logicAPI.RemoveBalls();
            ballsModel.Clear();
        }

        // Method to get a collection of ball models
        public override ObservableCollection<IBallModel> GetBallsModel()
        {
            return ballsModel;
        }
        
        private void ChangeModelBallsPositions(object sender, Tuple<Vector2, int, DateTime> e)
        {
            foreach (IBallModel model in ballsModel)
            {
                if (e.Item2 == model.Id)
                {
                    model.setPosition(e.Item1);
                }
            }   
        }
    }
}
