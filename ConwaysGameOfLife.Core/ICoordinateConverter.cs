namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Provide utilities such as coordinate conversion between single array and multi-dimension array.
    /// </summary>
    internal interface ICoordinateConverter
    {
        /// <summary>
        /// Convert a single array coordinate to a multi-dimensional array equivalent.
        /// </summary>
        /// <param name="dimension">Dimension of the output multi-dimensional array.</param>
        /// <param name="scale">Length of each dimension of the output multi-dimensional array.</param>
        /// <param name="singleDimensionCoordinate">Coordinate of the single dimensional array to convert.</param>
        /// <returns>Multi-dimensionalal array coordinate converted from the single dimensional array coordinate.</returns>
        int[] ConvertCoordinateSingleToMulti(int dimension, int scale, int singleDimensionCoordinate);

        /// <summary>
        /// Convert a multi-dimensional array coordinate to a single dimension array equivalent.
        /// </summary>
        /// <param name="scale">Length of each dimension of the multi-dimensional array.</param>
        /// <param name="multiDimensionCoordinate">Coordinate of the multi-dimensional array to convert.</param>
        /// <returns>Single dimensional array coorindate converted from the multi-dimensional array coordinate.</returns>
        int ConvertCoordinateMultiToSingle(int scale, int[] multiDimensionCoordinate);
    }
}
