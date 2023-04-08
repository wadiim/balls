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
                Vector2 maxVelocity = new Vector2(0.5f, 0.5f);
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

                // Handle collisions
                for (int i = 0; i < ballsCount; ++i)
                {
                    // Handle collisions between balls
                    for (int j = i + 1; j < ballsCount; ++j)
                    {
                        Vector2 firstBallVelocity = dataAPI.GetBallVelocity(i);
                        Vector2 secondBallVelocity = dataAPI.GetBallVelocity(j);

                        if (CollisionController.IsCollision(
                            dataAPI.GetBallPosition(i), firstBallVelocity, dataAPI.GetBallRadius(i),
                            dataAPI.GetBallPosition(j), secondBallVelocity, dataAPI.GetBallRadius(j)
                            ))
                        {
                            // Swap velocities
                            dataAPI.SetBallVelocity(i, secondBallVelocity);
                            dataAPI.SetBallVelocity(j, firstBallVelocity);
                        }
                    }

                    // Handle collision with a vertical wall
                    if (CollisionController.IsCollisionWithVerticalWall(
                        dataAPI.GetBallPosition(i), dataAPI.GetBallVelocity(i), dataAPI.GetBallRadius(i), dataAPI.GetTableWidth()
                        ))
                    {
                        Vector2 velocity = dataAPI.GetBallVelocity(i);
                        velocity.X *= -1.0f;
                        dataAPI.SetBallVelocity(i, velocity);
                    }

                    // Handle collision with a horizontal wall
                    if (CollisionController.IsCollisionWithHorizontalWall(
                        dataAPI.GetBallPosition(i), dataAPI.GetBallVelocity(i), dataAPI.GetBallRadius(i), dataAPI.GetTableHeight()
                        ))
                    {
                        Vector2 velocity = dataAPI.GetBallVelocity(i);
                        velocity.Y *= -1.0f;
                        dataAPI.SetBallVelocity(i, velocity);
                    }
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
