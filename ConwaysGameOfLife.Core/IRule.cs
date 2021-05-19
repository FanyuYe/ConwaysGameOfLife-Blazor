namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Apply rules to compute the next iteration of the world state in conway's game of life.
    /// </summary>
    internal interface IRule
    {
        /// <summary>
        /// Apply rules to get the state for the next iteration of provided cell from world.
        /// </summary>
        /// <param name="world">The world of the target cell. <seealso cref="IWorld"/></param>
        /// <param name="cellCoordinate">Coordinate of target cell being evaluated on.</param>
        /// <returns>State of the target cell for the next iteration.</returns>
        bool GetNextIterationOfCell(IWorld world, int cellCoordinate);
    }
}
