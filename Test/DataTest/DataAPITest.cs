using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Data;

namespace DataTest
{
    [TestClass]
    public class DataAPITest
    {
        [TestMethod]
        public void TestCreateBall()
        {
            DataAbstractAPI api = DataAbstractAPI.CreateDataAPI();
            Vector2 position = new Vector2(100, 100);
            Vector2 velocity = new Vector2(10, 10);

            IBall ball = api.CreateBall(position, velocity);

            Assert.AreEqual(position, ball.Position);
            Assert.AreEqual(velocity, ball.Velocity);
        }

        [TestMethod]
        public void TestGetBallsCount()
        {
            DataAbstractAPI api = DataAbstractAPI.CreateDataAPI();
            for (int i = 0; i < 5; ++i)
            {
                api.CreateBall(new Vector2(100, 100), new Vector2(10, 10));
            }

            int result = api.GetBallsCount();

            Assert.AreEqual(5, result);
        }
    }
}
