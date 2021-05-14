using System;
using System.Collections.Generic;
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
    }
}
