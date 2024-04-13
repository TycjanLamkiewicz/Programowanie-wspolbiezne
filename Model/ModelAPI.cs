// `Presentation` uses only the abstract `Logic` layer API
using Logic;
using System;
using System.Collections.ObjectModel;

namespace Model
{
    public abstract class ModelAPI
    {
        // Abstract method to get a collection of ball models
        public abstract ObservableCollection<IBallModel> GetBallsModel();

        // Abstract method to create balls in the model
        public abstract void CreateBalls(int num, int height, int width);

        // Abstract method to remove all balls from the model
        public abstract void RemoveAllBalls();

        // Abstract event triggered by model changes
        public abstract event EventHandler ModelEvent;

        // Static method to create an instance of ModelAPI
        public static ModelAPI CreateAPI()
        {
            return new Model(LogicAPI.CreateAPI());
        }
    }
}
