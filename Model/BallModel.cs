using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Model
{
    internal class BallModel : IBallModel, INotifyPropertyChanged
    {
        private float position_x;
        private float position_y;
        private int radius;

        public event PropertyChangedEventHandler PropertyChanged;

        public override float Position_x { get => position_x; set { position_x = value; NotifyPropertyChanged(); } }
        public override float Position_y { get => position_y; set { position_y = value; NotifyPropertyChanged(); } }
        public override int Radius { get => radius; }

        public BallModel(float position_x, float position_y, int radius) 
        { 
            this.position_x = position_x;
            this.position_y = position_y;
            this.radius = radius;
        }

        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
