using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Data;

namespace DataTest
{
    [TestClass]
    public class DataAPITest
    {
        [TestMethod]
        public void TestCreateBalls()
        {
            DataAbstractAPI api = DataAbstractAPI.CreateDataAPI();
            int expectedCount = 2;
            Vector2 maxPosition = new Vector2(100, 100);
            Vector2 maxVelocity = new Vector2(10, 10);

            api.CreateBalls(expectedCount, maxPosition, maxVelocity);
            int actualCount = api.GetBallsCount();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void TestGetBallsCount()
        {
            DataAbstractAPI api = DataAbstractAPI.CreateDataAPI();
            api.CreateBalls(5, new Vector2(100, 100), new Vector2(10, 10));

            int result = api.GetBallsCount();

            Assert.AreEqual(5, result);
        }
    }
}
