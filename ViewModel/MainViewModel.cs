using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Model;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ModelAPI modelAPI;
        private int numberOfBalls;          // Number of balls to create
        private bool isRunning = true;      // Flag indicating if the simulation is running or stopped

        // Collection of ball models obtained from the model layer
        public ObservableCollection<IBallModel> BallsModel => modelAPI.GetBallsModel();
        // Command to start the simulation
        public ICommand CommandStart { get; set; }
        // Command to reset the simulation
        public ICommand CommandReset { get; set; }

        // Constructor (using DI)
        public MainViewModel()
        {
            modelAPI = ModelAPI.CreateAPI();
            // Initialize command instances with appropriate methods
            CommandStart = new RelayCommand(Start, CanStart);
            CommandReset = new RelayCommand(Reset, CanReset);
        }

        // Property for the number of balls
        public int NumberOfBalls
        {
            get { return numberOfBalls; }
            set
            {
                // Update the number of balls and notify property change
                if (numberOfBalls != value)
                {
                    numberOfBalls = value;
                    OnPropertyChanged(nameof(NumberOfBalls));
                }
            }
        }

        // Property indicating if the simulation is running
        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                // Update the running flag and notify property change
                if (isRunning != value)
                {
                    isRunning = value;
                    OnPropertyChanged(nameof(IsRunning));
                }
            }
        }

        // Method to start the simulation
        private void Start(object parameter)
        {
            if (NumberOfBalls > 0)      // If there are balls to create
            {
                // Create balls in the model layer and update the view
                modelAPI.CreateBalls(NumberOfBalls, 400, 700);
                OnPropertyChanged(nameof(BallsModel));
                IsRunning = false;      // Set running flag to false
                // Notify commands to re-evaluate if they can be executed
                ((RelayCommand)CommandStart).OnCanExecuteChanged();
                ((RelayCommand)CommandReset).OnCanExecuteChanged();
            }
        }

        // Method to reset the simulation
        private void Reset(object parameter)
        {
            // Remove all balls from the model layer and update the view
            modelAPI.RemoveAllBalls();
            OnPropertyChanged(nameof(BallsModel));
            IsRunning = true;           // Set running flag to true
            // Notify commands to re-evaluate if they can be executed
            ((RelayCommand)CommandStart).OnCanExecuteChanged();
            ((RelayCommand)CommandReset).OnCanExecuteChanged();
        }

        // Method to check if the start command can be executed
        private bool CanStart(object parameter)
        {
            return IsRunning;
        }

        // Method to check if the reset command can be executed
        private bool CanReset(object parameter)
        {
            return !IsRunning;
        }

        // Event to notify of property changes
        public event PropertyChangedEventHandler PropertyChanged;
        // Method to raise the PropertyChanged event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
