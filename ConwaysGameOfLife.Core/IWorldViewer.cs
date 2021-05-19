namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Abstraction of viewing a world.
    /// </summary>
    internal interface IWorldViewer
    {
        /// <summary>
        /// Get dimension of the world.
        /// </summary>
        int Dimension { get; }

        /// <summary>
        /// Get scale of the world.
        /// </summary>
        int Scale { get; }

        /// <summary>
        /// Get state of a cell in the world by coordinate.
        /// </summary>
        /// <param name="cooridnate">Coordiante of the cell.</param>
        /// <returns></returns>
        bool GetState(int cooridnate);
    }
}
