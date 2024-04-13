using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        // Event for property change notification
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public override float Position_x { get => position_x; set { position_x = value; NotifyPropertyChanged(); } }
        public override float Position_y { get => position_y; set { position_y = value; NotifyPropertyChanged(); } }
        public override int Radius { get => radius; }

        // Constructor
        public BallModel(float position_x, float position_y, int radius) 
        { 
            this.position_x = position_x;
            this.position_y = position_y;
            this.radius = radius;
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
