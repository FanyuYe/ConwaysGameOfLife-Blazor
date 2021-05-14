using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ConwaysGameOfLife.Core.Tests
{
    /// <summary>
    /// TODO: 
    /// 1. One Assert per test.
    /// </summary>
    public class UtilityTest
    {
        #region Utility.ConvertCoordinateSingleToMulti

        [Fact]
        public void ConvertCoordinateSingleToMulti_ConvertTo1D_3_AllCorrect()
        {
            int dimension = 1;
            int scale = 3;
            int size = (int)Math.Pow(scale, dimension);

            for (int i = 0; i < size; ++i)
            {
                int[] x = Utility.ConvertCoordinateSingleToMulti(dimension, scale, i);
                var actual = x[0];
                var expected = i;

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void ConvertCoordinateSingleToMulti_ConvertTo2D_3x3_AllCorrect()
        {
            int dimension = 2;
            int scale = 3;
            int size = (int)Math.Pow(scale, dimension);
            var reference = new Dictionary<int, int[]>()
            {
                { 0, new int[2] { 0, 0 } },
                { 1, new int[2] { 1, 0 } },
                { 2, new int[2] { 2, 0 } },
                { 3, new int[2] { 0, 1 } },
                { 4, new int[2] { 1, 1 } },
                { 5, new int[2] { 2, 1 } },
                { 6, new int[2] { 0, 2 } },
                { 7, new int[2] { 1, 2 } },
                { 8, new int[2] { 2, 2 } },
            };

            for (int i = 0; i < size; ++i)
            {
                int[] xy = Utility.ConvertCoordinateSingleToMulti(dimension, scale, i);
                var actual = (xy[0], xy[1]);
                var expected = (reference[i][0], reference[i][1]);

                Assert.Equal(expected, actual);
            }
        }

        #endregion

        #region Utility.ConvertCoordinateMultiToSingle

        [Fact]
        public void ConvertCoordinateMultiToSingle_InvalidScale_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Utility.ConvertCoordinateMultiToSingle(3, new int[2] { 0, 3 }));
        }

        [Fact]
        public void ConvertCoordinateMultiToSingle_MultiDimensionCoordinateIsNull_ThrowsNullArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Utility.ConvertCoordinateMultiToSingle(3, null));
        }

        [Fact]
        public void ConvertCoordinateMultiToSingle_ConvertFrom1D_3_AllCorrect()
        {
            const int dimension = 1;
            int scale = 3;
            int size = (int)Math.Pow(scale, dimension);

            for (int i = 0; i < size; ++i)
            {
                int actual = Utility.ConvertCoordinateMultiToSingle(scale, new int[dimension] { i });
                var expected = i;

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void ConvertCoordinateMultiToSingle_ConvertFrom2D_3x3_AllCorrect()
        {
            int dimension = 2;
            int scale = 3;
            int size = (int)Math.Pow(scale, dimension);
            var reference = new Dictionary<int, int[]>()
            {
                { 0, new int[2] { 0, 0 } },
                { 1, new int[2] { 1, 0 } },
                { 2, new int[2] { 2, 0 } },
                { 3, new int[2] { 0, 1 } },
                { 4, new int[2] { 1, 1 } },
                { 5, new int[2] { 2, 1 } },
                { 6, new int[2] { 0, 2 } },
                { 7, new int[2] { 1, 2 } },
                { 8, new int[2] { 2, 2 } },
            };

            for (int i = 0; i < size; ++i)
            {
                int actual = Utility.ConvertCoordinateMultiToSingle(scale, reference[i]);
                var expected = i;

                Assert.Equal(expected, actual);
            }
        }

        #endregion

        #region Utility.GetNeighbourStatesFromCell

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
            int actualLeft = Utility.GetNeighbourStatesFromCell(worldMock.Object, left).Count(x => x);
            int actualRight = Utility.GetNeighbourStatesFromCell(worldMock.Object, right).Count(x => x);

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
            int actual = Utility.GetNeighbourStatesFromCell(worldMock.Object, center).Count(x => x);

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
            int actualTopLeft = Utility.GetNeighbourStatesFromCell(worldMock.Object, topLeft).Count(x => x);
            int actualTopRight = Utility.GetNeighbourStatesFromCell(worldMock.Object, topRight).Count(x => x);
            int actualBottomLeft = Utility.GetNeighbourStatesFromCell(worldMock.Object, bottomLeft).Count(x => x);
            int actualBottomRight = Utility.GetNeighbourStatesFromCell(worldMock.Object, bottomRight).Count(x => x);

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
            int actualTop = Utility.GetNeighbourStatesFromCell(worldMock.Object, top).Count(x => x);
            int actualLeft = Utility.GetNeighbourStatesFromCell(worldMock.Object, left).Count(x => x);
            int actualRight = Utility.GetNeighbourStatesFromCell(worldMock.Object, right).Count(x => x);
            int actualBottom = Utility.GetNeighbourStatesFromCell(worldMock.Object, bottom).Count(x => x);

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
            int actual = Utility.GetNeighbourStatesFromCell(worldMock.Object, center).Count(x => x);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
