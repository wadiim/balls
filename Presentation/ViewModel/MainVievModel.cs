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
        public int CanvasWidth { get; }
        public int CanvasHeight { get; }
        private string _InputText;

        public MainViewModel()
        {
            System.Diagnostics.Trace.WriteLine("Initializing MainViewModel");

            modelAPI = ModelAbstractAPI.CreaetModelAPI();
            Balls = new ObservableCollection<IBall>();

            CanvasWidth = modelAPI.GetCanvasWidth();
            CanvasHeight = modelAPI.GetCanvasHeight();

            StartButtonClick = new RelayCommand(() => StartButtonClickHandler());
        }

        private void StartButtonClickHandler()
        {
            System.Diagnostics.Trace.WriteLine("Handling start button click");

            int numOfBalls = ReadFromTextBox();
            modelAPI.StartSimulation(numOfBalls);

            for (int i = 0; i < numOfBalls; i++)
            {
                Balls.Add(modelAPI.GetBallModel(i));
            }
        }

        public string InputText
        {
            get => _InputText;
            set
            {
                _InputText = value;
                NotifyPropertyChanged(nameof(InputText));
            }
        }

        public int ReadFromTextBox()
        {
            if (int.TryParse(InputText, out _) && InputText != "0")
            {
                int number = int.Parse(InputText);
                System.Diagnostics.Trace.WriteLine("Number of balls read from input: " + number);
                return number > 10 ? 10 : number;
            }
            return 0;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}