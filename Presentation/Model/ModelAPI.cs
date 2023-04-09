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

        public abstract float GetCanvasWidth();
        public abstract float GetCanvasHeight();

        public static ModelAbstractAPI CreaetModelAPI(LogicAbstractAPI logicAPI = default)
        {
            return new ModelAPI(logicAPI ?? LogicAbstractAPI.CreateLogicAPI());
        }

        public abstract IDisposable Subscribe(IObserver<IBall> observer);

        private class ModelAPI : ModelAbstractAPI
        {
            private readonly static float Ratio = 4.5f;

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
                    float radius = logicAPI.GetBallRadius(value) * Ratio;

                    ball.Move((position.Y * Ratio) - radius, (position.X * Ratio) - radius);
                }
            }

            public override void StartSimulation(int numOfBalls)
            {
                logicAPI.StartSimulation(numOfBalls);

                for (int i = 0; i < numOfBalls; i++)
                {
                    Vector2 position = logicAPI.GetBallPosition(i);
                    float radius = logicAPI.GetBallRadius(i) * Ratio;
                    Balls.Add(new BallModel(i, (position.Y * Ratio) - radius, (position.X * Ratio) - radius, 2 * radius));
                }
            }

            public override IDisposable Subscribe(IObserver<IBall> observer)
            {
                this.observer = observer;
                return new Unsubscriber(this.observer);
            }

            public override float GetCanvasWidth()
            {
                return Ratio * logicAPI.GetTableWidth();
            }

            public override float GetCanvasHeight()
            {
                return Ratio * logicAPI.GetTableHeight();
            }
        }
    }
}
