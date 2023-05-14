using System;
using System.Numerics;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public abstract class LogicAbstractAPI : IObserver<IBall>, IObservable<int>
    {
        public abstract void StartSimulation(int numOfBalls);

        public abstract Vector2 GetBallPosition(int ballId);
        public abstract float GetBallRadius(int ballId);

        public abstract float GetTableWidth();
        public abstract float GetTableHeight();

        public abstract IDisposable Subscribe(IObserver<int> observer);

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(IBall ball);

        public static LogicAbstractAPI CreateLogicAPI(DataAbstractAPI dataAPI = default)
        {
            return new LogicAPI(dataAPI ?? DataAbstractAPI.CreateDataAPI());
        }

        private class LogicAPI : LogicAbstractAPI
        {
            private readonly DataAbstractAPI dataAPI;
            private readonly object ballLock = new object();
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
                return dataAPI.GetBall(ballId).Position;
            }

            public override float GetBallRadius(int ballId)
            {
                return dataAPI.GetBall(ballId).Radius;
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

                Random Rand = new Random();

                for (int i = 0; i < numOfBalls; ++i)
                {
                    IBall ball = dataAPI.CreateBall(
                        new Vector2(
                        ((float)Rand.NextDouble() * (maxPosition.X - 8.0F - float.Epsilon)) + (4.0F + float.Epsilon),
                        ((float)Rand.NextDouble() * (maxPosition.Y - 8.0F - float.Epsilon)) + (4.0F + float.Epsilon)
                        ),
                    new Vector2(
                        (float)Rand.NextDouble() * maxVelocity.X - (maxVelocity.X / 2),
                        (float)Rand.NextDouble() * maxVelocity.Y - (maxVelocity.Y / 2)
                        )
                    );
                    _ = ball.Subscribe(this);
                    _ = Task.Run(() => { ball.StartMoving(); });
                }
            }

            public override IDisposable Subscribe(IObserver<int> observer)
            {
                this.observer = observer;
                return new Unsubscriber(this.observer);
            }

            public override void OnCompleted()
            {
                throw new NotImplementedException();
            }

            public override void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public override void OnNext(IBall ball)
            {
                lock (ballLock)
                {
                    int ballsCount = dataAPI.GetBallsCount();
                    int index = -1;

                    // Handle collisions
                    for (int i = 0; i < ballsCount; ++i)
                    {
                        IBall other = dataAPI.GetBall(i);

                        if (ball == other)
                        {
                            index = i;
                            continue;
                        }

                        // Handle collisions between balls
                        if (CollisionController.IsCollision(ball.Position, ball.Velocity, ball.Radius, other.Position, other.Velocity, other.Radius))
                        {
                            // Swap velocities
                            Vector2 tmp = other.Velocity;
                            other.Velocity = ball.Velocity;
                            ball.Velocity = tmp;
                            observer.OnNext(i);
                        }

                        // Handle collision with a vertical wall
                        if (CollisionController.IsCollisionWithVerticalWall(ball.Position, ball.Velocity, ball.Radius, dataAPI.GetTableWidth()))
                        {
                            ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
                        }

                        // Handle collision with a horizontal wall
                        if (CollisionController.IsCollisionWithHorizontalWall(ball.Position, ball.Velocity, ball.Radius, dataAPI.GetTableHeight()))
                        {
                            ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                        }
                    }

                    observer.OnNext(index);
                }
            }
        }
    }
}
