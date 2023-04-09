using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using Logic;

namespace Model
{
    public interface IBall : INotifyPropertyChanged
    {
        int Id { get; }
        double Top { get; }
        double Left { get; }
        float Diameter { get; }
    }

    public abstract class ModelAbstractAPI : IObserver<int>, IObservable<IBall>
    {
        public abstract void OnCompleted();

        public abstract void OnError(Exception error);

        public abstract void OnNext(int value);

        public abstract void StartSimulation(int numOfBalls);

        public abstract IBall GetBallModel(int index);

        public abstract int GetCanvasWidth();
        public abstract int GetCanvasHeight();

        public static ModelAbstractAPI CreaetModelAPI(LogicAbstractAPI logicAPI = default)
        {
            return new ModelAPI(logicAPI ?? LogicAbstractAPI.CreateLogicAPI());
        }

        public abstract IDisposable Subscribe(IObserver<IBall> observer);

        private class ModelAPI : ModelAbstractAPI
        {
            private readonly LogicAbstractAPI logicAPI;
            private readonly List<BallModel> Balls = new List<BallModel>();
            private readonly IDisposable unsubscriber;
            private IObserver<IBall> observer;

            private class Unsubscriber : IDisposable
            {
                private IObserver<IBall> _observer;

                public Unsubscriber(IObserver<IBall> observer)
                {
                    _observer = observer;
                }

                public void Dispose()
                {
                    _observer = null;
                }
            }

            public ModelAPI(LogicAbstractAPI logicAPI)
            {
                System.Diagnostics.Trace.WriteLine("Initializing ModelAPI");

                this.logicAPI = logicAPI;
                unsubscriber = logicAPI.Subscribe(this);
            }

            public override IBall GetBallModel(int index)
            {
                return Balls[index];
            }

            public override void OnCompleted()
            {
                unsubscriber.Dispose();
            }

            public override void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public override void OnNext(int value)
            {
                if (value < Balls.Count)
                {
                    BallModel ball = Balls[value];
                    Vector2 position = logicAPI.GetBallPosition(value);
                    float diameter = 2 * logicAPI.GetBallRadius(value) * (GetCanvasWidth() / logicAPI.GetTableWidth());
                    double top = position.Y * GetCanvasHeight() / logicAPI.GetTableHeight() - (diameter / 2);
                    double left = position.X * GetCanvasWidth() / logicAPI.GetTableWidth() - (diameter / 2);

                    System.Diagnostics.Trace.WriteLine("ModelAPI: Updating Balls[" + value + "] position: (" + top + ", " + left + ")");
                    ball.Move(top, left);
                }
            }

            public override void StartSimulation(int numOfBalls)
            {
                System.Diagnostics.Trace.WriteLine("ModelAPI: Starting simulation with " + numOfBalls + " balls");

                logicAPI.StartSimulation(numOfBalls);

                for (int i = 0; i < numOfBalls; i++)
                {
                    Vector2 position = logicAPI.GetBallPosition(i);
                    float diameter = 2 * logicAPI.GetBallRadius(i) * (GetCanvasWidth() / logicAPI.GetTableWidth());
                    double top = position.Y * GetCanvasHeight() / logicAPI.GetTableHeight() - (diameter / 2);
                    double left = position.X * GetCanvasWidth() / logicAPI.GetTableWidth() - (diameter / 2);

                    System.Diagnostics.Trace.WriteLine("ModelAPI: Creating BallModel: Id = " + i + "; Position = (" + top + ", " + left + ")");
                    BallModel ball = new BallModel(i, top, left, diameter);
                    Balls.Add(ball);
                }
            }

            public override IDisposable Subscribe(IObserver<IBall> observer)
            {
                this.observer = observer;
                return new Unsubscriber(this.observer);
            }

            public override int GetCanvasWidth()
            {
                return 576; // 128 * 4.5
            }

            public override int GetCanvasHeight()
            {
                return 432; // 96 * 4.5
            }
        }
    }
}
