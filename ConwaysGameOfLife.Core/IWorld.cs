using System.Collections.Generic;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Properties describe a conway's game of life world.
    /// </summary>
    public interface IWorld
    {
        /// <summary>
        /// Dimension of the world.
        /// </summary>
        public int Dimension { get; set; }

        /// <summary>
        /// Length of a single dimension for the world.
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// State of the world. True is live and false is dead.
        /// </summary>
        public bool[] State { get; set; }
    }
}
