using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Model;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ModelAbstractAPI modelAPI;
        public ObservableCollection<IBall> Balls { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand StartButtonClick { get; set; }
        public float CanvasWidth { get; }
        public float CanvasHeight { get; }
        public string Input { get; set; }
        private bool _isIdle;

        public bool IsIdle
        {
            get => _isIdle;
            set
            {
                _isIdle = value;
                NotifyPropertyChanged();
            }
        }

        public MainViewModel()
        {
            modelAPI = ModelAbstractAPI.CreaetModelAPI();
            Balls = new ObservableCollection<IBall>();

            CanvasWidth = modelAPI.GetCanvasWidth();
            CanvasHeight = modelAPI.GetCanvasHeight();

            Input = "1";
            IsIdle = true;

            StartButtonClick = new RelayCommand(() => StartButtonClickHandler());
        }

        private void StartButtonClickHandler()
        {
            IsIdle = false;
            int numOfBalls = ReadInput();
            modelAPI.StartSimulation(numOfBalls);

            for (int i = 0; i < numOfBalls; i++)
            {
                Balls.Add(modelAPI.GetBallModel(i));
            }
        }

        public int ReadInput()
        {
            return int.TryParse(Input, out _) && Input != "0" ? int.Parse(Input) : 0;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}