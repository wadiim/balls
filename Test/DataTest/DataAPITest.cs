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
        public void TestGetBallVelocity()
        {
            DataAbstractAPI api = DataAbstractAPI.CreateDataAPI();
            int ballId = 0;
            Vector2 expectedVelocity = new Vector2(5, 5);
            Vector2 maxPosition = new Vector2(100, 100);
            Vector2 maxVelocity = new Vector2(10, 10);
            api.CreateBalls(1, maxPosition, maxVelocity);
            api.SetBallVelocity(ballId, expectedVelocity);

            Vector2 actualVelocity = api.GetBallVelocity(ballId);

            Assert.AreEqual(expectedVelocity, actualVelocity);
        }

        [TestMethod]
        public void TestSetBallVelocity()
        {
            DataAbstractAPI api = DataAbstractAPI.CreateDataAPI();
            int ballId = 0;
            Vector2 expectedVelocity = new Vector2(5, 5);
            Vector2 maxPosition = new Vector2(100, 100);
            Vector2 maxVelocity = new Vector2(10, 10);
            api.CreateBalls(1, maxPosition, maxVelocity);

            api.SetBallVelocity(ballId, expectedVelocity);
            Vector2 actualVelocity = api.GetBallVelocity(ballId);

            Assert.AreEqual(expectedVelocity, actualVelocity);
        }

        [TestMethod]
        public void TestUpdateBallPosition()
        {
            var dataAPI = DataAbstractAPI.CreateDataAPI();
            dataAPI.CreateBalls(1, new Vector2(50, 50), new Vector2(10, 10));
            var initialPosition = dataAPI.GetBallPosition(0);
            var initialVelocity = dataAPI.GetBallVelocity(0);

            dataAPI.UpdateBallPosition(0);

            Assert.AreNotEqual(initialPosition, dataAPI.GetBallPosition(0));
            Assert.AreEqual(initialPosition + initialVelocity, dataAPI.GetBallPosition(0));
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
