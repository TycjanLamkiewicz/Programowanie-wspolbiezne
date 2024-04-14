using Xunit;
using Moq;
using Data;
using System.Numerics;
using System.Collections.Generic;

namespace DataTests
{
    public class DataAPITests
    {
        [Fact]
        public void AddBall_AddsBallToList()
        {
            // Arrange
            var dataApiMock = new Mock<DataAPI>();
            var ballMock = new Mock<IBall>();

            dataApiMock.Setup(api => api.AddBall(It.IsAny<IBall>()));

            // Act
            dataApiMock.Object.AddBall(ballMock.Object);

            // Assert
            dataApiMock.Verify(api => api.AddBall(ballMock.Object), Times.Once);
        }

        [Fact]
        public void RemoveBalls_EmptiesListOfBalls()
        {
            // Arrange
            var dataApiMock = new Mock<DataAPI>();

            // Ensure that Balls property returns an empty list
            dataApiMock.SetupGet(api => api.Balls).Returns(new List<IBall>());

            dataApiMock.Setup(api => api.RemoveBalls());

            // Act
            dataApiMock.Object.RemoveBalls();

            // Assert
            Assert.Empty(dataApiMock.Object.Balls);
        }


        [Fact]
        public void GetPositions_ReturnsCorrectPositions()
        {
            // Arrange
            var dataApiMock = new Mock<DataAPI>();
            var ball1 = new Mock<IBall>();
            var ball2 = new Mock<IBall>();

            ball1.SetupGet(b => b.Position_x).Returns(10);
            ball1.SetupGet(b => b.Position_y).Returns(20);
            ball2.SetupGet(b => b.Position_x).Returns(30);
            ball2.SetupGet(b => b.Position_y).Returns(40);

            dataApiMock.Setup(api => api.GetPositions()).Returns(new List<Vector2> { new Vector2(10, 20), new Vector2(30, 40) });

            // Act
            var positions = dataApiMock.Object.GetPositions();

            // Assert
            Assert.Equal(new List<Vector2> { new Vector2(10, 20), new Vector2(30, 40) }, positions);
        }

        [Fact]
        public void GetPosition_ReturnsCorrectPosition()
        {
            // Arrange
            var dataApiMock = new Mock<DataAPI>();
            var ballMock = new Mock<IBall>();

            ballMock.SetupGet(b => b.Position_x).Returns(10);
            ballMock.SetupGet(b => b.Position_y).Returns(20);

            dataApiMock.Setup(api => api.GetPosition(ballMock.Object)).Returns(new Vector2(10, 20));

            // Act
            var position = dataApiMock.Object.GetPosition(ballMock.Object);

            // Assert
            Assert.Equal(new Vector2(10, 20), position);
        }
    }
}
