namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Simulating the conway's game of life.
    /// </summary>
    internal interface ISimulator
    {
        /// <summary>
        /// Envolve the game into next iteration.
        /// </summary>
        void Tick();
    }
}
