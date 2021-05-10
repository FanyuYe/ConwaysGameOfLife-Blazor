namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Simulating the conway's game of life.
    /// </summary>
    public interface ISimulator
    {
        /// <summary>
        /// Envolve the world into next iteration.
        /// </summary>
        /// <param name="world">The world being evaluated on.</param>
        public void GoToNextIteration(IWorld world);
    }
}
