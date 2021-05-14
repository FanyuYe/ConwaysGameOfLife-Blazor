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
            int dimension = 1;
            int scale = 3;
            int size = (int)Math.Pow(scale, dimension);
            int left = 0;
            int right = 2;

            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, dimension);
            worldMock.SetupProperty(world => world.Scale, scale);
            worldMock.SetupProperty(world => world.State, Enumerable.Repeat(true, size).ToArray());

            int expected = 1;
            int actualLeft = worldInterpreter.GetNeighbourStatesFromCell(worldMock.Object, left).Count(x => x);
            int actualRight = worldInterpreter.GetNeighbourStatesFromCell(worldMock.Object, right).Count(x => x);

            Assert.Equal(expected, actualLeft);
            Assert.Equal(expected, actualRight);
        }

        [Fact]
        public void GetNeighbourStatesFromCell_1D_3_CenterWithAllLiveNeighbours_ReturnValueContainsTwoTrue()
        {
            int dimension = 1;
            int scale = 3;
            int size = (int)Math.Pow(scale, dimension);
            int center = 1;

            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, dimension);
            worldMock.SetupProperty(world => world.Scale, scale);
            worldMock.SetupProperty(world => world.State, Enumerable.Repeat(true, size).ToArray());

            int expected = 2;
            int actual = worldInterpreter.GetNeighbourStatesFromCell(worldMock.Object, center).Count(x => x);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNeighbourStatesFromCell_2D_3x3_FourCornerWithAllLiveNeighbours_ReturnValueContainsThreeTrue()
        {
            int dimension = 2;
            int scale = 3;
            int size = scale * scale;
            int topLeft = 0;
            int topRight = 2;
            int bottomLeft = 6;
            int bottomRight = 8;

            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, dimension);
            worldMock.SetupProperty(world => world.Scale, scale);
            worldMock.SetupProperty(world => world.State, Enumerable.Repeat(true, size).ToArray());

            int expected = 3;
            int actualTopLeft = worldInterpreter.GetNeighbourStatesFromCell(worldMock.Object, topLeft).Count(x => x);
            int actualTopRight = worldInterpreter.GetNeighbourStatesFromCell(worldMock.Object, topRight).Count(x => x);
            int actualBottomLeft = worldInterpreter.GetNeighbourStatesFromCell(worldMock.Object, bottomLeft).Count(x => x);
            int actualBottomRight = worldInterpreter.GetNeighbourStatesFromCell(worldMock.Object, bottomRight).Count(x => x);

            Assert.Equal(expected, actualTopLeft);
            Assert.Equal(expected, actualTopRight);
            Assert.Equal(expected, actualBottomLeft);
            Assert.Equal(expected, actualBottomRight);
        }

        [Fact]
        public void GetNeighbourStatesFromCell_2D_3x3_FourCenterOfEdgeWithAllLiveNeighbours_ReturnValueContainsFiveTrue()
        {
            int dimension = 2;
            int scale = 3;
            int size = scale * scale;
            int top = 1;
            int left = 3;
            int right = 5;
            int bottom = 7;

            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, dimension);
            worldMock.SetupProperty(world => world.Scale, scale);
            worldMock.SetupProperty(world => world.State, Enumerable.Repeat(true, size).ToArray());

            int expected = 5;
            int actualTop = worldInterpreter.GetNeighbourStatesFromCell(worldMock.Object, top).Count(x => x);
            int actualLeft = worldInterpreter.GetNeighbourStatesFromCell(worldMock.Object, left).Count(x => x);
            int actualRight = worldInterpreter.GetNeighbourStatesFromCell(worldMock.Object, right).Count(x => x);
            int actualBottom = worldInterpreter.GetNeighbourStatesFromCell(worldMock.Object, bottom).Count(x => x);

            Assert.Equal(expected, actualTop);
            Assert.Equal(expected, actualLeft);
            Assert.Equal(expected, actualRight);
            Assert.Equal(expected, actualBottom);
        }

        [Fact]
        public void GetNeighbourStatesFromCell_2D_3x3_CenterOfSquareWithAllLiveNeighbours_ReturnValueContainsEightTrue()
        {
            int dimension = 2;
            int scale = 3;
            int size = scale * scale;
            int center = 4;

            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, dimension);
            worldMock.SetupProperty(world => world.Scale, scale);
            worldMock.SetupProperty(world => world.State, Enumerable.Repeat(true, size).ToArray());

            int expected = 8;
            int actual = worldInterpreter.GetNeighbourStatesFromCell(worldMock.Object, center).Count(x => x);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
