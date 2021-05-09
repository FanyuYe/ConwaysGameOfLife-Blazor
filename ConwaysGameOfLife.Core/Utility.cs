using System;
using System.Collections.Generic;
using System.Linq;

namespace ConwaysGameOfLife.Core
{
    public static class Utility
    {
        private static Dictionary<int, IEnumerable<int[]>> offsetMatrixCache = new Dictionary<int, IEnumerable<int[]>>();

        public static int[] ConvertCoordinateSingleToMulti(int dimension, int scale, int singleDimensionCoordinate)
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

        public static int ConvertCoordinateMultiToSingle(int dimension, int scale, int[] multiDimensionCoordinate)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<bool> GetNeighbourStatesFromCell(this IWorld world, int targetCell)
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

            int[] coordinate = ConvertCoordinateSingleToMulti(world.Dimension, world.Scale, targetCell);

            if (!offsetMatrixCache.TryGetValue(world.Dimension, out IEnumerable<int[]> permutations))
            {
                permutations = CreateOffsetMatrix(world.Dimension);
                // Cause tests failure, need to be fixed
                //offsetMatrixCache.Add(world.Dimension, permutations);
            }

            foreach (int[] p in permutations)
            {
                bool isOutOfRange = false;
                int index = 0;

                for (int dim = 0; dim < world.Dimension; ++dim)
                {
                    p[dim] += coordinate[dim];

                    if (p[dim] < 0 || p[dim] > world.Scale - 1)
                    {
                        isOutOfRange = true;
                        break;
                    }
                    else
                    {
                        index += p[dim] * (int)Math.Pow(world.Scale, world.Dimension - dim - 1);
                    }
                }

                if (isOutOfRange)
                {
                    yield return false;
                }
                else
                {
                    yield return world.State[index];
                }
            }
        }
    }
}
