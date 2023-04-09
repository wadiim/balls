using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Model
{
    public class BallModel : IBall
    {
        public int Id { get; }

        private double _Top;
        public double Top
        {
            get => _Top;
            set
            {
                if (_Top == value)
                {
                    return;
                }

                _Top = value;
                NotifyPropertyChanged();
            }
        }

        private double _Left;
        public double Left
        {
            get => _Left;
            set
            {
                if (_Left == value)
                {
                    return;
                }

                _Left = value;
                NotifyPropertyChanged();
            }
        }

        public float Diameter { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BallModel(int id, double top, double left, float diameter)
        {
            Id = id;
            Top = top;
            Left = left;
            Diameter = diameter;
        }

        public void Move(double top, double left)
        {
            Top = top;
            Left = left;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
