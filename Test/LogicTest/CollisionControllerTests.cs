using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Logic;

namespace LogicTest
{
    [TestClass]
    public class CollisionControllerTests
    {
        [TestMethod]
        public void TestIsCollision_True_LeftWall()
        {
            Vector2 ballPosition = new Vector2(5, 50);
            Vector2 ballVelocity = new Vector2(-10, 0);
            float ballRadius = 10;
            float boardWidth = 100;

            bool result = CollisionController.IsCollisionWithVerticalWall(ballPosition, ballVelocity, ballRadius, boardWidth);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsCollision_True_RightWall()
        {
            Vector2 ballPosition = new Vector2(95, 50);
            Vector2 ballVelocity = new Vector2(10, 0);
            float ballRadius = 10;
            float boardWidth = 100;

            bool result = CollisionController.IsCollisionWithVerticalWall(ballPosition, ballVelocity, ballRadius, boardWidth);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsCollision_True_TopWall()
        {
            Vector2 ballPosition = new Vector2(50, 5);
            Vector2 ballVelocity = new Vector2(0, -10);
            float ballRadius = 10;
            float boardHeight = 100;

            bool result = CollisionController.IsCollisionWithHorizontalWall(ballPosition, ballVelocity, ballRadius, boardHeight);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsCollision_True_BottomWall()
        {
            Vector2 ballPosition = new Vector2(50, 95);
            Vector2 ballVelocity = new Vector2(0, 10);
            float ballRadius = 10;
            float boardHeight = 100;

            bool result = CollisionController.IsCollisionWithHorizontalWall(ballPosition, ballVelocity, ballRadius, boardHeight);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsCollision_False()
        {
            Vector2 ballPosition = new Vector2(50, 50);
            Vector2 ballVelocity = new Vector2(10, 10);
            float ballRadius = 10;
            float boardWidth = 100;
            float boardHeight = 100;

            Assert.IsFalse(CollisionController.IsCollisionWithVerticalWall(ballPosition, ballVelocity, ballRadius, boardWidth));
            Assert.IsFalse(CollisionController.IsCollisionWithHorizontalWall(ballPosition, ballVelocity, ballRadius, boardHeight));
        }

        [TestMethod]
        public void TestIsCollision_BallsColliding_ReturnsTrue()
        {
            Vector2 firstBallPosition = new Vector2(100, 100);
            Vector2 firstBallVelocity = new Vector2(10, 0);
            float firstBallRadius = 10;
            Vector2 secondBallPosition = new Vector2(115, 100);
            Vector2 secondBallVelocity = new Vector2(-10, 0);
            float secondBallRadius = 10;

            bool result = CollisionController.IsCollision(firstBallPosition, firstBallVelocity, firstBallRadius,
                                                           secondBallPosition, secondBallVelocity, secondBallRadius);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsCollision_BallsNotColliding_ReturnsFalse()
        {
            Vector2 firstBallPosition = new Vector2(100, 100);
            Vector2 firstBallVelocity = new Vector2(-10, 0);
            float firstBallRadius = 10;
            Vector2 secondBallPosition = new Vector2(130, 100);
            Vector2 secondBallVelocity = new Vector2(10, 0);
            float secondBallRadius = 10;

            bool result = CollisionController.IsCollision(firstBallPosition, firstBallVelocity, firstBallRadius,
                                                           secondBallPosition, secondBallVelocity, secondBallRadius);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestIsCollision_BallsOverlappingButNotColliding_ReturnsFalse()
        {
            Vector2 firstBallPosition = new Vector2(100, 100);
            Vector2 firstBallVelocity = new Vector2(-10, 0);
            float firstBallRadius = 10;
            Vector2 secondBallPosition = new Vector2(105, 100);
            Vector2 secondBallVelocity = new Vector2(10, 0);
            float secondBallRadius = 10;

            bool result = CollisionController.IsCollision(firstBallPosition, firstBallVelocity, firstBallRadius,
                                                           secondBallPosition, secondBallVelocity, secondBallRadius);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestIsCollision_BallsMovingApart_ReturnsFalse()
        {
            Vector2 firstBallPosition = new Vector2(100, 100);
            Vector2 firstBallVelocity = new Vector2(10, 0);
            float firstBallRadius = 10;
            Vector2 secondBallPosition = new Vector2(115, 100);
            Vector2 secondBallVelocity = new Vector2(10, 0);
            float secondBallRadius = 10;

            bool result = CollisionController.IsCollision(firstBallPosition, firstBallVelocity, firstBallRadius,
                                                           secondBallPosition, secondBallVelocity, secondBallRadius);

            Assert.IsFalse(result);
        }
    }
}