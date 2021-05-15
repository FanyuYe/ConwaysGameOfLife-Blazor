using System;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// World represents and describes a conways's game of life.
    /// </summary>
    public class World : IWorld
    {
        /// <inheritdoc/>
        public int Dimension { get; }

        /// <inheritdoc/>
        public int Scale { get; }

        /// <inheritdoc/>
        public bool[] State { get; }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="dimension">Dimension of the world.</param>
        /// <param name="scale">Length of a single dimension of the world.</param>
        /// <param name="seed">Seed used as initial state of the world.</param>
        public World(int dimension, int scale, bool[] seed = null)
        {
            (Dimension, Scale) = (dimension, scale);

            int size = (int)Math.Pow(scale, dimension);

            if (seed == null)
            {
                State = new bool[size];
            }
            else if (seed.Length == size)
            {
                State = (bool[])seed.Clone();
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    $"Seed's length is {seed.Length}, which is incompatible with provided dimension ({dimension}) and scale ({scale}).");
            }
        }
    }
}
