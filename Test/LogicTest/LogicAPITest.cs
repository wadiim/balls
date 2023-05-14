using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;
using System;
using Logic;
using Data;

namespace LogicTest
{
    public class FakeBall : IBall
    {
        public override Vector2 Position => _Position;
        public override Vector2 Velocity { get; set; }
        public override float Radius { get; }

        private Vector2 _Position;
        private readonly IList<IObserver<IBall>> observers;

        public FakeBall(Vector2 position, Vector2 velocity)
        {
            observers = new List<IObserver<IBall>>();

            _Position = position;
            Velocity = velocity;
            Radius = 1f;
        }

        public override void StartMoving()
        {
            _Position += Velocity;
        }

        public override IDisposable Subscribe(IObserver<IBall> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return null;
        }
    }

    public class FakeDataAPI : DataAbstractAPI
    {
        private readonly List<IBall> Balls;

        public FakeDataAPI()
        {
            Balls = new List<IBall>();
        }

        public override IBall CreateBall(Vector2 position, Vector2 velocity)
        {
            IBall ball = new FakeBall(position, velocity);
            Balls.Add(ball);
            return ball;
        }

        public override IBall GetBall(int index)
        {
            return Balls[index];
        }

        public override int GetBallsCount()
        {
            return Balls.Count;
        }

        public override float GetTableHeight()
        {
            return 32f;
        }

        public override float GetTableWidth()
        {
            return 64f;
        }
    }

    [TestClass]
    public class LogicAPITest
    {
        private DataAbstractAPI fakeDataAPI;
        private LogicAbstractAPI logicAPI;

        [TestInitialize]
        public void SetUp()
        {
            fakeDataAPI = new FakeDataAPI();
            logicAPI = LogicAbstractAPI.CreateLogicAPI(fakeDataAPI);
        }

        [TestMethod]
        public void CreateLogicAPI_DefaultDataAPI_ReturnsLogicAPIInstance()
        {
            Assert.IsInstanceOfType(logicAPI, typeof(LogicAbstractAPI));
        }

        [TestMethod]
        public void StartSimulation_CreatesBallsWithRandomPosition()
        {
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
        public void TestGetBallPosition()
        {
            Vector2 position = new Vector2(10f, 10f);
            Vector2 velocity = new Vector2(5f, 5f);
            _ = fakeDataAPI.CreateBall(position, velocity);

            Assert.AreEqual(position, logicAPI.GetBallPosition(0));
        }

        [TestMethod]
        public void TestGetBallRadius()
        {
            Vector2 position = new Vector2(10f, 10f);
            Vector2 velocity = new Vector2(5f, 5f);
            IBall ball = fakeDataAPI.CreateBall(position, velocity);

            Assert.AreEqual(ball.Radius, logicAPI.GetBallRadius(0));
        }

        [TestMethod]
        public void TestGetTableWidth()
        {
            Assert.AreEqual(fakeDataAPI.GetTableWidth(), logicAPI.GetTableWidth());
        }

        [TestMethod]
        public void TestGetTableHeight()
        {
            Assert.AreEqual(fakeDataAPI.GetTableHeight(), logicAPI.GetTableHeight());
        }
    }
}