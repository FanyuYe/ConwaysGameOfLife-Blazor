using System.Collections.Generic;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Being able to interpret the world of conway's game of life such as neighbour states of the target cell.
    /// </summary>
    internal interface IWorldInterpreter
    {
        /// <summary>
        /// Returns neighbour states of target cell.
        /// </summary>
        /// <param name="world">World that provide information such as dimension and scale. <seealso cref="IWorld"/></param>
        /// <param name="targetCell">Target cell coordinate where neighbours are based on.</param>
        /// <returns>Neighbour states as IEnumerable.</returns>
        IEnumerable<bool> GetNeighbourStatesFromCell(IWorld world, int targetCell);
    }
}
