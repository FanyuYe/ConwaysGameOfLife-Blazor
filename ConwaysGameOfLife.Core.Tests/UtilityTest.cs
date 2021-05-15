using System;
using Xunit;

namespace ConwaysGameOfLife.Core.Tests
{
    public class UtilityTest
    {
        #region Utility.ConvertCoordinateSingleToMulti

        [Theory]
        // 1D 3
        [InlineData(0, new int[1] { 0 }, 3)]
        [InlineData(1, new int[1] { 1 }, 3)]
        [InlineData(2, new int[1] { 2 }, 3)]
        // 2D 3*3
        [InlineData(0, new int[2] { 0, 0 }, 3)]
        [InlineData(1, new int[2] { 1, 0 }, 3)]
        [InlineData(2, new int[2] { 2, 0 }, 3)]
        [InlineData(3, new int[2] { 0, 1 }, 3)]
        [InlineData(4, new int[2] { 1, 1 }, 3)]
        [InlineData(5, new int[2] { 2, 1 }, 3)]
        [InlineData(6, new int[2] { 0, 2 }, 3)]
        [InlineData(7, new int[2] { 1, 2 }, 3)]
        [InlineData(8, new int[2] { 2, 2 }, 3)]
        public void ConvertCoordinateSingleToMulti_CorrectReturnValue(int originCoordinate, int[] expected, int scale)
        {
            int[] actual = Utility.ConvertCoordinateSingleToMulti(expected.Length, scale, originCoordinate);

            Assert.Equal(expected, actual);
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

        [Theory]
        // 1D 3
        [InlineData(new int[1] { 0 }, 0, 3)]
        [InlineData(new int[1] { 1 }, 1, 3)]
        [InlineData(new int[1] { 2 }, 2, 3)]
        // 2D 3*3
        [InlineData(new int[2] { 0, 0 }, 0, 3)]
        [InlineData(new int[2] { 1, 0 }, 1, 3)]
        [InlineData(new int[2] { 2, 0 }, 2, 3)]
        [InlineData(new int[2] { 0, 1 }, 3, 3)]
        [InlineData(new int[2] { 1, 1 }, 4, 3)]
        [InlineData(new int[2] { 2, 1 }, 5, 3)]
        [InlineData(new int[2] { 0, 2 }, 6, 3)]
        [InlineData(new int[2] { 1, 2 }, 7, 3)]
        [InlineData(new int[2] { 2, 2 }, 8, 3)]
        public void ConvertCoordinateMultiToSingle_CorrectReturnValue(int[] originCoordinate, int expected, int scale)
        {
            int actual = Utility.ConvertCoordinateMultiToSingle(scale, originCoordinate);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
