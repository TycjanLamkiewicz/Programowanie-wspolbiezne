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
        private int num;

        // Constructor (using DI)
        public Model(LogicAPI logicAPI)
        {
            this.logicAPI = logicAPI;
            logicAPI.LogicEvent += ChangeModelBallsPositions;
        }

        // Method to create balls in the model
        public override void CreateBalls(int num, int height, int width)
        {
            this.num = num;
            logicAPI.CreateBalls(num, height, width);
        }

        // Method to remove all balls from the model
        public override void RemoveAllBalls()
        {
            logicAPI.RemoveBalls();
        }

        // Method to get a collection of ball models
        public override ObservableCollection<IBallModel> GetBallsModel()
        {
            ballsModel.Clear();                     // Clear the existing ball models collection
            int radius = 25;                        // Radius of the balls

            // Iterate through each ball position from LogicAPI and create corresponding ball models
            foreach (Vector2 ball in logicAPI.GetPositions())
            {
                IBallModel ballModel = IBallModel.CreateBallModel(ball.X, ball.Y, radius);
                ballsModel.Add(ballModel);          // Add the ball model to the collection
            }

            return ballsModel;                      // Return the collection of ball models
        }

        private void ChangeModelBallsPositions(object sender, EventArgs e)
        {
            int i = 0;

            // Update the positions of ball models in the collection based on the positions from LogicAPI
            foreach (Vector2 ball in logicAPI.GetPositions())
            {
                if (num == ballsModel.Count)        // Check if the number of ball models matches the expected count
                {
                    ballsModel[i].Position_x = ball.X;  // Update positions
                    ballsModel[i].Position_y = ball.Y;
                    i++;
                }
            }
            
        }
    }
}
