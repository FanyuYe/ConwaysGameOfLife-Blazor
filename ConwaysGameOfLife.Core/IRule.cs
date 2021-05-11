namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Apply rules to compute the next iteration of the world state in conway's game of life.
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// Apply rules to compute whether a particular cell should survive, die or spwan in next iteration.
        /// </summary>
        /// <param name="world">The world of the target cell. <seealso cref="IWorld"/></param>
        /// <param name="cellCoordinate">Coordinate of target cell being evaluated on.</param>
        /// <returns>State of the target cell for the next iteration.</returns>
        public bool ApplyRuleToCell(IWorld world, int cellCoordinate);
    }
}
