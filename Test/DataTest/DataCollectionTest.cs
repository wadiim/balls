using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Data;

namespace DataTest
{
    [TestClass]
    public class BallCollectionTests
    {
        [TestMethod]
        public void TestCreateBall()
        {
            Vector2 position = new Vector2(100, 100);
            Vector2 velocity = new Vector2(10, 10);
            BallCollection ballCollection = new BallCollection();

            Ball ball = ballCollection.CreateBall(position, velocity);

            Assert.AreEqual(1, ballCollection.Count());
            Assert.AreEqual(position, ball.Position);
            Assert.AreEqual(velocity, ball.Velocity);
        }

        [TestMethod]
        public void TestGetBall()
        {
            BallCollection ballCollection = new BallCollection();
            Ball created = ballCollection.CreateBall(new Vector2(100, 100), new Vector2(10, 10));

            Ball returned = ballCollection.GetBall(0);

            Assert.IsNotNull(returned);
            Assert.IsInstanceOfType(returned, typeof(Ball));
            Assert.AreEqual(created, returned);
        }

        [TestMethod]
        public void TestCount()
        {
            BallCollection ballCollection = new BallCollection();
            for (int i = 0; i < 5; ++i)
            {
                _ = ballCollection.CreateBall(new Vector2(100, 100), new Vector2(10, 10));
            }

            Assert.AreEqual(5, ballCollection.Count());
        }
    }
}
