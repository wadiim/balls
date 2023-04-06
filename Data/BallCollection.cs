using System;
using System.Collections.Generic;
using System.Numerics;

namespace Data
{
    public class BallCollection
    {
        private readonly List<Ball> Balls;
        private static readonly Random Rand = new Random();

        public BallCollection()
        {
            Balls = new List<Ball>();
        }

        public void CreateBalls(int NumOfBalls, Vector2 maxPosition, Vector2 maxVelocity)
        {
            for (int i = 0; i < NumOfBalls; ++i)
            {
                Balls.Add(new Ball(
                    i,
                    new Vector2(
                        (float)Rand.NextDouble() * maxPosition.X,
                        (float)Rand.NextDouble() * maxPosition.Y
                        ),
                    new Vector2(
                        (float)Rand.NextDouble() * maxVelocity.X,
                        (float)Rand.NextDouble() * maxVelocity.Y
                        )
                    ));
            }
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