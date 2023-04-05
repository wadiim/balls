using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Data;

namespace DataTest
{
    [TestClass]
    public class BallTest
    {
        [TestMethod]
        public void TestGetIdIfFirstInstanceThenIdIsEqualTo0()
        {
            Ball ball = new Ball(new Vector2(0, 0), new Vector2(0, 0));
            Assert.AreEqual(0, ball.Id);
        }

        [TestMethod]
        public void TestGetIdIfNextInstanceThenTheIdIsIncremented()
        {
            Ball ball = new Ball(new Vector2(0, 0), new Vector2(0, 0));
            Assert.AreEqual(1, ball.Id);
        }

        [TestMethod]
        public void TestUpdatePositionWhetherMovesByVelocity()
        {
            Ball ball = new Ball(new Vector2(0, 0), new Vector2(2, 4));
            Assert.AreEqual(new Vector2(0, 0), ball.Position);
            ball.UpdatePosition();
            Assert.AreEqual(new Vector2(2, 4), ball.Position);
        }

        [TestMethod]
        public void TestUpdatePositionIfMultipleConsecutiveCallsThenMovesBySumOfAllTheVectors()
        {
            Ball ball = new Ball(new Vector2(0, 0), new Vector2(2, 7));
            Assert.AreEqual(new Vector2(0, 0), ball.Position);
            ball.UpdatePosition();
            Assert.AreEqual(new Vector2(2, 7), ball.Position);
            ball.UpdatePosition();
            Assert.AreEqual(new Vector2(4, 14), ball.Position);
            ball.Velocity = new Vector2(1, 1);
            ball.UpdatePosition();
            Assert.AreEqual(new Vector2(5, 15), ball.Position);
        }
    }
}
