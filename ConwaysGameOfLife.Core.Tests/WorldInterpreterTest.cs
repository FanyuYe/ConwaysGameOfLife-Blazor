using Moq;
using System;
using System.Linq;
using Xunit;

namespace ConwaysGameOfLife.Core.Tests
{
    /// <summary>
    /// TODO: 
    /// 1. One Assert per test.
    /// </summary>
    public class WorldInterpreterTest
    {
        #region Init

        private readonly WorldInterpreter worldInterpreter;

        public WorldInterpreterTest()
        {
            worldInterpreter = new WorldInterpreter();
        }

        #endregion

        #region GetNeighbourStatesFromCell(IWorld, int)

        [Fact]
        public void GetNeighbourStatesFromCell_1D_3_TwoEndsWithAllLiveNeighbours_ReturnValueContainsOneTrue()
        {
            int left = 0;
            int right = 2;
            var world = TestHelper.CreateMockWorld1D_3(Enumerable.Repeat(true, 3).ToArray());

            int expected = 1;
            int actualLeft = worldInterpreter.GetNeighbourStatesFromCell(world, left).Count(x => x);
            int actualRight = worldInterpreter.GetNeighbourStatesFromCell(world, right).Count(x => x);

            Assert.Equal(expected, actualLeft);
            Assert.Equal(expected, actualRight);
        }

        [Fact]
        public void GetNeighbourStatesFromCell_1D_3_CenterWithAllLiveNeighbours_ReturnValueContainsTwoTrue()
        {
            int center = 1;
            var world = TestHelper.CreateMockWorld1D_3(Enumerable.Repeat(true, 3).ToArray());

            int expected = 2;
            int actual = worldInterpreter.GetNeighbourStatesFromCell(world, center).Count(x => x);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNeighbourStatesFromCell_2D_3x3_FourCornerWithAllLiveNeighbours_ReturnValueContainsThreeTrue()
        {
            int topLeft = 0;
            int topRight = 2;
            int bottomLeft = 6;
            int bottomRight = 8;
            var world = TestHelper.CreateMockWorld2D_3x3(Enumerable.Repeat(true, 9).ToArray());

            int expected = 3;
            int actualTopLeft = worldInterpreter.GetNeighbourStatesFromCell(world, topLeft).Count(x => x);
            int actualTopRight = worldInterpreter.GetNeighbourStatesFromCell(world, topRight).Count(x => x);
            int actualBottomLeft = worldInterpreter.GetNeighbourStatesFromCell(world, bottomLeft).Count(x => x);
            int actualBottomRight = worldInterpreter.GetNeighbourStatesFromCell(world, bottomRight).Count(x => x);

            Assert.Equal(expected, actualTopLeft);
            Assert.Equal(expected, actualTopRight);
            Assert.Equal(expected, actualBottomLeft);
            Assert.Equal(expected, actualBottomRight);
        }

        [Fact]
        public void GetNeighbourStatesFromCell_2D_3x3_FourCenterOfEdgeWithAllLiveNeighbours_ReturnValueContainsFiveTrue()
        {
            int top = 1;
            int left = 3;
            int right = 5;
            int bottom = 7;
            var world = TestHelper.CreateMockWorld2D_3x3(Enumerable.Repeat(true, 9).ToArray());

            int expected = 5;
            int actualTop = worldInterpreter.GetNeighbourStatesFromCell(world, top).Count(x => x);
            int actualLeft = worldInterpreter.GetNeighbourStatesFromCell(world, left).Count(x => x);
            int actualRight = worldInterpreter.GetNeighbourStatesFromCell(world, right).Count(x => x);
            int actualBottom = worldInterpreter.GetNeighbourStatesFromCell(world, bottom).Count(x => x);

            Assert.Equal(expected, actualTop);
            Assert.Equal(expected, actualLeft);
            Assert.Equal(expected, actualRight);
            Assert.Equal(expected, actualBottom);
        }

        [Fact]
        public void GetNeighbourStatesFromCell_2D_3x3_CenterOfSquareWithAllLiveNeighbours_ReturnValueContainsEightTrue()
        {
            int center = 4;
            var world = TestHelper.CreateMockWorld2D_3x3(Enumerable.Repeat(true, 9).ToArray());

            int expected = 8;
            int actual = worldInterpreter.GetNeighbourStatesFromCell(world, center).Count(x => x);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
