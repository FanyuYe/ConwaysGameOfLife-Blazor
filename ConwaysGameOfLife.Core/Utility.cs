using System;
using System.Collections.Generic;
using System.Linq;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Provide utilities such as coordinate conversion between single array and multi-dimension array as well as querying neighbours.
    /// </summary>
    internal static class Utility
    {
        private static Dictionary<int, IEnumerable<int[]>> offsetMatrixCache = new Dictionary<int, IEnumerable<int[]>>();

        /// <summary>
        /// Convert a single array coordinate to a multi-dimensional array equivalent.
        /// <para>Note: Least significant dimension is the first, most significant dimension is the last.</para>
        /// <para>Example: Coordinate 7 of scale 3 converts to 2-dimensional coordinate (1, 2). i.e. 1 + 2 * 3</para>
        /// </summary>
        /// <param name="dimension">Dimension of the output multi-dimensional array.</param>
        /// <param name="scale">Length of each dimension of the output multi-dimensional array.</param>
        /// <param name="singleDimensionCoordinate">Coordinate of the single dimensional array to convert.</param>
        /// <returns>Multi-dimensionalal array coordinate converted from the single dimensional array coordinate.</returns>
        internal static int[] ConvertCoordinateSingleToMulti(int dimension, int scale, int singleDimensionCoordinate)
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
        internal static int ConvertCoordinateMultiToSingle(int scale, int[] multiDimensionCoordinate)
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

        /// <summary>
        /// Returns an enumrable that contains states from the target coordinate neighbours.
        /// <para>Neighbour is defined as offset on each dimension from the target coordinate no greater than 1. Excluding self.</para>
        /// </summary>
        /// <param name="world">World that provide information such as dimension and scale. <seealso cref="IWorld"/></param>
        /// <param name="targetCell">Target cell coordinate where neighbours are based on.</param>
        /// <returns></returns>
        internal static IEnumerable<bool> GetNeighbourStatesFromCell(this IWorld world, int targetCell)
        {
            IEnumerable<int[]> CreateOffsetMatrix(int dimension)
            {
                var permutations = new List<int[]>() { new int[dimension] };

                for (int dim = 0; dim < dimension; ++dim)
                {
                    foreach (int[] p in permutations.ToList())
                    {
                        var p1 = (int[])p.Clone();
                        var p2 = (int[])p.Clone();
                        p1[dim] = -1;
                        p2[dim] = 1;
                        permutations.Add(p1);
                        permutations.Add(p2);
                    }
                }

                permutations.RemoveAt(0);

                return permutations;
            }

            int[] multiCoo = ConvertCoordinateSingleToMulti(world.Dimension, world.Scale, targetCell);

            if (!offsetMatrixCache.TryGetValue(world.Dimension, out IEnumerable<int[]> permutations))
            {
                permutations = CreateOffsetMatrix(world.Dimension);
                offsetMatrixCache.TryAdd(world.Dimension, permutations);
            }

            foreach (int[] p in permutations)
            {
                bool isOutOfRange = false;
                int[] neighbourCoo = new int[world.Dimension];

                for (int dim = 0; dim < world.Dimension; ++dim)
                {
                    neighbourCoo[dim] = multiCoo[dim] + p[dim];

                    if (neighbourCoo[dim] < 0 || neighbourCoo[dim] > world.Scale - 1)
                    {
                        isOutOfRange = true;
                        break;
                    }
                }

                if (isOutOfRange)
                {
                    yield return false;
                }
                else
                {
                    int i = ConvertCoordinateMultiToSingle(world.Scale, neighbourCoo);
                    yield return world.State[i];
                }
            }
        }
    }
}
