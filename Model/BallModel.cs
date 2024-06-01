using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Model
{
    // Class implementing the IBallModel interface and providing property change notification
    internal class BallModel : IBallModel, INotifyPropertyChanged
    {
        // Private fields
        private float position_x;
        private float position_y;
        private int radius;
        private int id;

        // Event for property change notification
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public override float Position_x
        {
            get
            {
                lock (lock_position)
                {
                    return position_x;
                }
            }
        }
        public override float Position_y
        {
            get
            {
                lock (lock_position)
                {
                    return position_y;
                }
            }
        }
        public override int Radius { get => radius; }
        public override int Id { get => id; }

        // Constructor
        public BallModel(int radius, int id) 
        { 
            this.radius = radius;
            this.id = id;
        }

        private readonly object lock_position = new object();
        public override void setPosition(Vector2 position)
        {
            lock(lock_position)
            {
                position_x = position.X;
                position_y = position.Y;
                NotifyPropertyChanged("Position_x");
                NotifyPropertyChanged("Position_y");
            }
            

        }

        // The propertyName parameter is optional and will be automatically populated with the name of the property that has been changed,
        // thanks to the [CallerMemberName] attribute. If no property name is passed, an empty value is used.
        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = "")
        {
            // If PropertyChanged is not null (i.e. has subscribers), the Invoke method will be called, passing the current object(this) as the sender
            // and a new PropertyChangedEventArgs object containing the name of the changed property. This name is passed as the propertyName parameter.
            // This allows objects subscribing to the PropertyChanged event to read which property has been changed and take appropriate action in response to the change.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
    }
}
