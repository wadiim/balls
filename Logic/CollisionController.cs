using System.Numerics;

namespace Logic
{
    public class CollisionController
    {
        public static bool IsCollisionWithVerticalWall(Vector2 ballPosition, Vector2 ballVelocity, float ballRadius, float boardWidth)
        {
            // Calculate the position of the ball at the next time step
            Vector2 nextPosition = ballPosition + ballVelocity;

            // Check if the ball will hit the left or right wall at the next time step
            return nextPosition.X - ballRadius <= 0 || nextPosition.X + ballRadius >= boardWidth;
        }

        public static bool IsCollisionWithHorizontalWall(Vector2 ballPosition, Vector2 ballVelocity, float ballRadius, float boardHeight)
        {
            // Calculate the position of the ball at the next time step
            Vector2 nextPosition = ballPosition + ballVelocity;

            // Check if the ball will hit the top or bottom wall at the next time step
            return nextPosition.Y - ballRadius <= 0 || nextPosition.Y + ballRadius >= boardHeight;
        }


        public static bool IsCollision(Vector2 firstBallPosition,  Vector2 firstBallVelocity,  float firstBallRadius,
                                       Vector2 secondBallPosition, Vector2 secondBallVelocity, float secondBallRadius)
        {
            // Calculate the distance between the centers of the two balls
            Vector2 centerDistance = secondBallPosition - firstBallPosition;
            float distance = centerDistance.Length();

            // Calculate the sum of the radii of the two balls
            float radiiSum = firstBallRadius + secondBallRadius;

            // If the balls are overlaping or they adhere to each other, then they are potentially colliding
            if (distance <= radiiSum)
            {
                // Check if the balls are moving towards each other
                Vector2 relativeVelocity = secondBallVelocity - firstBallVelocity;
                float dotProduct = Vector2.Dot(relativeVelocity, centerDistance);
                if (dotProduct < 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
