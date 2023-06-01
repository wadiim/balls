using System.Collections.Generic;
using System.Numerics;

namespace Data
{
    internal class BallCollection
    {
        private readonly List<Ball> Balls;

        public BallCollection()
        {
            Balls = new List<Ball>();
        }

        public Ball CreateBall(Vector2 position, Vector2 velocity)
        {
            Ball ball = new Ball(position, velocity);
            Balls.Add(ball);
            return ball;
        }

        public Ball GetBall(int index)
        {
            return Balls[index];
        }

        public int Count()
        {
            return Balls.Count;
        }
    }
}