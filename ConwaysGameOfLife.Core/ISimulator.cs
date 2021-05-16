namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Simulating the conway's game of life.
    /// </summary>
    public interface ISimulator
    {
        /// <summary>
        /// Envolve the game into next iteration.
        /// </summary>
        /// <param name="world">The world being simulated.</param>
        void Tick(IWorld world);
    }
}
