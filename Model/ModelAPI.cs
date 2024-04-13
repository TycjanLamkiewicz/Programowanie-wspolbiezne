using Logic;
using System;
using System.Collections.ObjectModel;

namespace Model
{
    public abstract class ModelAPI
    {
        public abstract ObservableCollection<IBallModel> GetBallsModel();
        public abstract void CreateBalls(int num, int height, int width);
        public abstract void RemoveAllBalls();
        public abstract event EventHandler ModelEvent;

        public static ModelAPI CreateAPI() 
        { 
            return new Model(LogicAPI.CreateAPI());
        }
    }
}
