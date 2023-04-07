using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Logic;

namespace LogicTest
{
    [TestClass]
    public class LogicAPITest
    {
        [TestMethod]
        public void CreateLogicAPI_DefaultDataAPI_ReturnsLogicAPIInstance()
        {
            var logicAPI = LogicAbstractAPI.CreateLogicAPI();
            Assert.IsInstanceOfType(logicAPI, typeof(LogicAbstractAPI));
        }

        [TestMethod]
        public void StartSimulation_CreatesBallsWithRandomPosition()
        {
            var logicAPI = LogicAbstractAPI.CreateLogicAPI();
            int numOfBalls = 5;

            logicAPI.StartSimulation(numOfBalls);

            for (int i = 0; i < numOfBalls; i++)
            {
                Vector2 position = logicAPI.GetBallPosition(i);
                Assert.IsTrue(position.X >= 0);
                Assert.IsTrue(position.X <= logicAPI.GetTableWidth());
                Assert.IsTrue(position.Y >= 0);
                Assert.IsTrue(position.Y <= logicAPI.GetTableHeight());
            }
        }

        [TestMethod]
        public void UpdateSimulation_UpdatesBallPositions()
        {
            var logicAPI = LogicAbstractAPI.CreateLogicAPI();
            int numOfBalls = 20;
            logicAPI.StartSimulation(numOfBalls);

            logicAPI.UpdateSimulation();

            for (int i = 0; i < numOfBalls; i++)
            {
                Vector2 position = logicAPI.GetBallPosition(i);
                if (position.X < 0 || position.X > logicAPI.GetTableWidth() || position.Y < 0 || position.Y > logicAPI.GetTableHeight())
                {
                    logicAPI.UpdateSimulation();
                }
                Assert.IsTrue(position.X >= 0);
                Assert.IsTrue(position.X <= logicAPI.GetTableWidth());
                Assert.IsTrue(position.Y >= 0);
                Assert.IsTrue(position.Y <= logicAPI.GetTableHeight());
            }
        }
    }
}