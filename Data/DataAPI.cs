using System;
using System.Numerics;

namespace Data
{
    public abstract class IBall : IObservable<IBall>
    {
        public abstract Vector2 Position { get; }
        public abstract Vector2 Velocity { get; set; }
        public abstract float Radius { get; }

        public abstract void StartMoving();

        public abstract IDisposable Subscribe(IObserver<IBall> observer);
    }

    public abstract class DataAbstractAPI
    {
        public abstract void CreateBalls(int NumOfBalls, Vector2 maxPosition, Vector2 maxVelocity);
        public abstract int GetBallsCount();
        public abstract IBall GetBall(int index);

        public abstract float GetTableWidth();
        public abstract float GetTableHeight();

        public static DataAbstractAPI CreateDataAPI()
        {
            return new DataAPI();
        }

        private class DataAPI : DataAbstractAPI
        {
            private readonly BallCollection Balls;
            private readonly Table Table;

            public DataAPI()
            {
                Balls = new BallCollection();
                Table = new Table(128, 96);
            }

            public override void CreateBalls(int NumOfBalls, Vector2 maxPosition, Vector2 maxVelocity)
            {
                Balls.CreateBalls(NumOfBalls, maxPosition, maxVelocity);
            }

            public override int GetBallsCount()
            {
                return Balls.Count();
            }

            public override IBall GetBall(int index)
            {
                return Balls.GetBall(index);
            }

            public override float GetTableWidth()
            {
                return Table.Width;
            }

            public override float GetTableHeight()
            {
                return Table.Height;
            }
        }
    }
}
