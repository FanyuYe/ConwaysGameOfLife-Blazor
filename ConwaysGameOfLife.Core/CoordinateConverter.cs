using System;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Base implementation of a coordinate converter between single and multi-dimensional arrays.
    /// </summary>
    internal class CoordinateConverter : ICoordinateConverter
    {
        /// <summary>
        /// Convert a single array coordinate to a multi-dimensional array equivalent.
        /// <para>Note: Least significant dimension is the first, most significant dimension is the last.</para>
        /// <para>Example: Coordinate 7 of scale 3 converts to 2-dimensional coordinate (1, 2). i.e. 1 + 2 * 3</para>
        /// </summary>
        /// <param name="dimension">Dimension of the output multi-dimensional array.</param>
        /// <param name="scale">Length of each dimension of the output multi-dimensional array.</param>
        /// <param name="singleDimensionCoordinate">Coordinate of the single dimensional array to convert.</param>
        /// <returns>Multi-dimensionalal array coordinate converted from the single dimensional array coordinate.</returns>
        public int[] ConvertCoordinateSingleToMulti(int dimension, int scale, int singleDimensionCoordinate)
        {
            int[] coordinate = new int[dimension];
            int remainder = singleDimensionCoordinate;
            int multiplier;

            for (int dim = 0; dim < dimension; ++dim)
            {
                multiplier = (int)Math.Pow(scale, dimension - dim - 1);
                coordinate[dimension - dim - 1] = remainder / multiplier;
                remainder %= multiplier;
            }

            return coordinate;
        }

        /// <summary>
        /// Convert a multi-dimensional array coordinate to a single dimension array equivalent.
        /// <para>Note: Least significant dimension is the first, most significant dimension is the last.</para>
        /// <para>Example: Coordinate (1, 2) of scale 3 converts to single dimensional coordinate 7. i.e. 1 + 2 * 3</para>
        /// </summary>
        /// <param name="scale">Length of each dimension of the multi-dimensional array.</param>
        /// <param name="multiDimensionCoordinate">Coordinate of the multi-dimensional array to convert.</param>
        /// <returns>Single dimensional array coorindate converted from the multi-dimensional array coordinate.</returns>
        public int ConvertCoordinateMultiToSingle(int scale, int[] multiDimensionCoordinate)
        {
            if (multiDimensionCoordinate == null)
                throw new ArgumentNullException($"Coordinate is null.");

            int dimension = multiDimensionCoordinate.Length;
            int coo = 0;

            for (int dim = 0; dim < dimension; ++dim)
            {
                int i = multiDimensionCoordinate[dim];
                if (i < 0 || i > scale - 1)
                    throw new ArgumentException($"Provided scale is {scale} while coordinate[{dim}] is {i}.");

                coo += i * (int)Math.Pow(scale, dim);
            }

            return coo;
        }
    }
}
