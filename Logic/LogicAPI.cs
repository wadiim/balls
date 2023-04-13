using System;
using System.Numerics;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public abstract class LogicAbstractAPI : IObservable<int>
    {
        public abstract void StartSimulation(int numOfBalls);
        public abstract void UpdateSimulation();

        public abstract Vector2 GetBallPosition(int ballId);
        public abstract float GetBallRadius(int ballId);

        public abstract float GetTableWidth();
        public abstract float GetTableHeight();

        public abstract IDisposable Subscribe(IObserver<int> observer);

        public static LogicAbstractAPI CreateLogicAPI(DataAbstractAPI dataAPI = default)
        {
            return new LogicAPI(dataAPI ?? DataAbstractAPI.CreateDataAPI());
        }

        private class LogicAPI : LogicAbstractAPI
        {
            private readonly DataAbstractAPI dataAPI;
            private Task simulation;
            private IObserver<int> observer = null;
            private static readonly Random Rand = new Random();
            private static readonly Vector2 maxVelocity = new Vector2(0.01f, 0.01f);

            private class Unsubscriber : IDisposable
            {
                private IObserver<int> _observer;

                public Unsubscriber(IObserver<int> observer)
                {
                    this._observer = observer;
                }

                public void Dispose()
                {
                    _observer = null;
                }
            }

            public LogicAPI(DataAbstractAPI dataAPI)
            {
                this.dataAPI = dataAPI;
            }

            public override Vector2 GetBallPosition(int ballId)
            {
                return dataAPI.GetBallPosition(ballId);
            }

            public override float GetBallRadius(int ballId)
            {
                return dataAPI.GetBallRadius(ballId);
            }

            public override float GetTableWidth()
            {
                return dataAPI.GetTableWidth();
            }

            public override float GetTableHeight()
            {
                return dataAPI.GetTableHeight();
            }

            public override void StartSimulation(int numOfBalls)
            {
                Vector2 maxPosition = new Vector2(dataAPI.GetTableWidth(), dataAPI.GetTableHeight());
                dataAPI.CreateBalls(numOfBalls, maxPosition, maxVelocity);

                // Run the simulation asynchronously
                simulation = Task.Run(RunSimulation);
            }

            private void RunSimulation()
            {
                while (true)
                {
                    UpdateSimulation();
                }
            }

            public override void UpdateSimulation()
            {
                int ballsCount = dataAPI.GetBallsCount();

                // Update ball positions
                for (int i = 0; i < ballsCount; ++i)
                {
                    Vector2 velocity = dataAPI.GetBallVelocity(i);
                    velocity.X = ((float)Rand.NextDouble() * 2 * maxVelocity.X) - maxVelocity.X;
                    velocity.Y = ((float)Rand.NextDouble() * 2 * maxVelocity.Y) - maxVelocity.Y;

                    // Handle collision with a vertical wall
                    if (CollisionController.IsCollisionWithVerticalWall(
                        dataAPI.GetBallPosition(i), velocity, dataAPI.GetBallRadius(i), dataAPI.GetTableWidth()
                        ))
                    {
                        velocity.X *= -1.0f;
                    }

                    // Handle collision with a horizontal wall
                    if (CollisionController.IsCollisionWithHorizontalWall(
                        dataAPI.GetBallPosition(i), velocity, dataAPI.GetBallRadius(i), dataAPI.GetTableHeight()
                        ))
                    {
                        velocity.Y *= -1.0f;
                    }

                    dataAPI.SetBallVelocity(i, velocity);
                }

                // Update position of each ball
                for (int i = 0; i < ballsCount; ++i)
                {
                    dataAPI.UpdateBallPosition(i);

                    // Notify the observer that the position of the i-th ball has changed
                    if (observer != null)
                    {
                        observer.OnNext(i);
                    }
                }
            }

            public override IDisposable Subscribe(IObserver<int> observer)
            {
                this.observer = observer;
                return new Unsubscriber(this.observer);
            }
        }
    }
}
