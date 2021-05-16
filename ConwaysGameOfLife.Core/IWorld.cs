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
        int Dimension { get; }

        /// <summary>
        /// Length of a single dimension for the world.
        /// </summary>
        int Scale { get; }

        /// <summary>
        /// State of the world. True is live and false is dead.
        /// </summary>
        bool[] State { get; }
    }
}
