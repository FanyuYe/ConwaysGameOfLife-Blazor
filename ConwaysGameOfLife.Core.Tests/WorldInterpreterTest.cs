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
            worldInterpreter = new WorldInterpreter(TestHelper.CreateMockCoordinateConverter(3));
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
