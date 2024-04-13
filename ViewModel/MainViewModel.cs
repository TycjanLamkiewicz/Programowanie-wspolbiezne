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
        private int numberOfBalls;
        private bool isRunning = true;

        public ObservableCollection<IBallModel> BallsModel => modelAPI.GetBallsModel();
        public ICommand CommandStart { get; set; }
        public ICommand CommandReset { get; set; }

        public MainViewModel()
        {
            modelAPI = ModelAPI.CreateAPI();
            CommandStart = new RelayCommand(Start, CanStart);
            CommandReset = new RelayCommand(Reset, CanReset);
        }

        public int NumberOfBalls
        {
            get { return numberOfBalls; }
            set
            {
                if (numberOfBalls != value)
                {
                    numberOfBalls = value;
                    OnPropertyChanged(nameof(NumberOfBalls));
                }
            }
        }

        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    OnPropertyChanged(nameof(IsRunning));
                }
            }
        }

        private void Start(object parameter)
        {
            if (NumberOfBalls > 0) 
            { 
                modelAPI.CreateBalls(NumberOfBalls, 400, 700);
                OnPropertyChanged(nameof(BallsModel));
                IsRunning = false;
                ((RelayCommand)CommandStart).OnCanExecuteChanged();
                ((RelayCommand)CommandReset).OnCanExecuteChanged();
            }
        }

        private void Reset(object parameter)
        {
            modelAPI.RemoveAllBalls();
            OnPropertyChanged(nameof(BallsModel));
            IsRunning = true;
            ((RelayCommand)CommandStart).OnCanExecuteChanged();
            ((RelayCommand)CommandReset).OnCanExecuteChanged();
        }

        private bool CanStart(object parameter)
        {
            return IsRunning;
        }

        private bool CanReset(object parameter)
        {
            return !IsRunning;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
