using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Data;

namespace DataTest
{
    [TestClass]
    public class BallCollectionTests
    {
        [TestMethod]
        public void TestCreateBalls()
        {
            int numOfBalls = 10;
            Vector2 maxPosition = new Vector2(100, 100);
            Vector2 maxVelocity = new Vector2(10, 10);
            BallCollection ballCollection = new BallCollection();

            ballCollection.CreateBalls(numOfBalls, maxPosition, maxVelocity);

            Assert.AreEqual(numOfBalls, ballCollection.Count());
            for (int i = 0; i < numOfBalls; i++)
            {
                Ball ball = ballCollection.GetBall(i);
                Assert.IsNotNull(ball);
                Assert.IsTrue(ball.Position.X >= 0 && ball.Position.X <= maxPosition.X);
                Assert.IsTrue(ball.Position.Y >= 0 && ball.Position.Y <= maxPosition.Y);
                Assert.IsTrue(ball.Velocity.X >= -maxVelocity.X && ball.Velocity.X <= maxVelocity.X);
                Assert.IsTrue(ball.Velocity.Y >= -maxVelocity.Y && ball.Velocity.Y <= maxVelocity.Y);
            }
        }

        [TestMethod]
        public void TestGetBall()
        {
            BallCollection ballCollection = new BallCollection();
            ballCollection.CreateBalls(3, new Vector2(100, 100), new Vector2(10, 10));

            Ball ball = ballCollection.GetBall(2);

            Assert.IsNotNull(ball);
            Assert.IsInstanceOfType(ball, typeof(Ball));
            Assert.AreEqual(2, ball.Id);
        }

        [TestMethod]
        public void TestCount()
        {
            BallCollection ballCollection = new BallCollection();
            ballCollection.CreateBalls(2, new Vector2(100, 100), new Vector2(10, 10));

            int result = ballCollection.Count();

            Assert.AreEqual(2, result);
        }
    }
}
