using System.Collections.Generic;
using System.Linq;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Provide utilities such as coordinate conversion between single array and multi-dimension array as well as querying neighbours.
    /// <para>Neighbour is defined as offset on each dimension from the target coordinate no greater than 1. Excluding self.</para>
    /// </summary>
    public class WorldInterpreter : IWorldInterpreter
    {
        private readonly Dictionary<int, IEnumerable<int[]>> offsetMatrixCache = new Dictionary<int, IEnumerable<int[]>>();

        /// <inheritdoc/>
        public IEnumerable<bool> GetNeighbourStatesFromCell(IWorld world, int targetCell)
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

            int[] multiCoo = Utility.ConvertCoordinateSingleToMulti(world.Dimension, world.Scale, targetCell);

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
                    int i = Utility.ConvertCoordinateMultiToSingle(world.Scale, neighbourCoo);
                    yield return world.State[i];
                }
            }
        }
    }
}
