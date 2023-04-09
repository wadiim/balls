using System.Numerics;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        public abstract Vector2 GetBallPosition(int ballId);
        public abstract Vector2 GetBallVelocity(int ballId);
        public abstract void SetBallVelocity(int ballId, Vector2 velocity);
        public abstract float GetBallMass(int ballId);
        public abstract float GetBallRadius(int ballId);
        public abstract void UpdateBallPosition(int ballId);

        public abstract float GetTableWidth();
        public abstract float GetTableHeight();

        public abstract void CreateBalls(int NumOfBalls, Vector2 maxPosition, Vector2 maxVelocity);
        public abstract int GetBallsCount();

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

            public override float GetBallMass(int ballId)
            {
                return Balls.GetBall(ballId).Mass;
            }

            public override Vector2 GetBallPosition(int ballId)
            {
                return Balls.GetBall(ballId).Position;
            }

            public override float GetBallRadius(int ballId)
            {
                return Balls.GetBall(ballId).Radius;
            }

            public override int GetBallsCount()
            {
                return Balls.Count();
            }

            public override Vector2 GetBallVelocity(int ballId)
            {
                return Balls.GetBall(ballId).Velocity;
            }

            public override float GetTableWidth()
            {
                return Table.Width;
            }

            public override float GetTableHeight()
            {
                return Table.Height;
            }

            public override void SetBallVelocity(int ballId, Vector2 velocity)
            {
                Balls.GetBall(ballId).Velocity = velocity;
            }

            public override void UpdateBallPosition(int ballId)
            {
                Balls.GetBall(ballId).UpdatePosition();
            }
        }
    }
}
