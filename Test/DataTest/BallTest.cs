using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Threading;
using System.Numerics;
using Data;

namespace DataTest
{
    [TestClass]
    public class BallTest
    {
        [TestMethod]
        public void TestStartMoving()
        {
            Ball ball = new Ball(new Vector2(0, 0), new Vector2(2, 4));
            Assert.AreEqual(new Vector2(0, 0), ball.Position);
            _ = Task.Run(ball.StartMoving);
            Thread.Sleep(50);
            Assert.AreNotEqual(new Vector2(0, 0), ball.Position);
        }
    }
}
