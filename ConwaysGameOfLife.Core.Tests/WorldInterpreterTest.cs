using Moq;
using System;
using System.Linq;
using Xunit;

namespace ConwaysGameOfLife.Core.Tests
{
    public class WorldInterpreterTest
    {
        #region Init

        private readonly WorldInterpreter worldInterpreter;

        public WorldInterpreterTest()
        {
            var converterMock = new Mock<ICoordinateConverter>(MockBehavior.Strict);
            // 1D 3
            converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(3, new int[1] { 0 })).Returns(0);
            converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(3, new int[1] { 1 })).Returns(1);
            converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(3, new int[1] { 2 })).Returns(1);
            converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(1, 3, 0)).Returns(new int[1] { 0 });
            converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(1, 3, 1)).Returns(new int[1] { 1 });
            converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(1, 3, 2)).Returns(new int[1] { 2 });
            // 2D 3*3
            converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(3, new int[2] { 0, 0 })).Returns(0);
            converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(3, new int[2] { 1, 0 })).Returns(1);
            converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(3, new int[2] { 2, 0 })).Returns(2);
            converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(3, new int[2] { 0, 1 })).Returns(3);
            converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(3, new int[2] { 1, 1 })).Returns(4);
            converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(3, new int[2] { 2, 1 })).Returns(5);
            converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(3, new int[2] { 0, 2 })).Returns(6);
            converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(3, new int[2] { 1, 2 })).Returns(7);
            converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(3, new int[2] { 2, 2 })).Returns(8);
            converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(2, 3, 0)).Returns(new int[2] { 0, 0 });
            converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(2, 3, 1)).Returns(new int[2] { 1, 0 });
            converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(2, 3, 2)).Returns(new int[2] { 2, 0 });
            converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(2, 3, 3)).Returns(new int[2] { 0, 1 });
            converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(2, 3, 4)).Returns(new int[2] { 1, 1 });
            converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(2, 3, 5)).Returns(new int[2] { 2, 1 });
            converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(2, 3, 6)).Returns(new int[2] { 0, 2 });
            converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(2, 3, 7)).Returns(new int[2] { 1, 2 });
            converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(2, 3, 8)).Returns(new int[2] { 2, 2 });

            worldInterpreter = new WorldInterpreter(converterMock.Object);
        }

        #endregion

        #region WorldInterpreter(ICoordinateConverter)
        
        [Fact]
        public void WorldInterpreter_ParameterCoordinateConverterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WorldInterpreter(null));
        }

        #endregion

        #region GetNeighbourStatesFromCell(IWorld, int)

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        public void GetNeighbourStatesFromCell_1D_3_AllLiveNeighbours_CheckReturnAliveNeighbourSameAsExpected(int targetCoordinate, int aliveNeighbourCount)
        {
            var world = TestHelper.CreateMockWorld1D_3(Enumerable.Repeat(true, 3).ToArray());

            int actual = worldInterpreter.GetNeighbourStatesFromCell(world, targetCoordinate).Count(x => x);

            Assert.Equal(aliveNeighbourCount, actual);
        }

        [Theory]
        [InlineData(0, 3)]
        [InlineData(1, 5)]
        [InlineData(2, 3)]
        [InlineData(3, 5)]
        [InlineData(4, 8)]
        [InlineData(5, 5)]
        [InlineData(6, 3)]
        [InlineData(7, 5)]
        [InlineData(8, 3)]
        public void GetNeighbourStatesFromCell_2D_3x3_AllLiveNeighbours_CheckReturnAliveNeighbourSameAsExpected(int targetCoordinate, int aliveNeighbourCount)
        {
            var world = TestHelper.CreateMockWorld2D_3x3(Enumerable.Repeat(true, 9).ToArray());

            int actual = worldInterpreter.GetNeighbourStatesFromCell(world, targetCoordinate).Count(x => x);

            Assert.Equal(aliveNeighbourCount, actual);
        }

        #endregion
    }
}
