using System;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Controls conway's game of life.
    /// </summary>
    public class GameController : IGameController
    {
        private readonly IWorld world;
        private readonly ISimulator simulator;
        private readonly bool[] seed;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="world">World the game is based on.</param>
        /// <param name="simulator">Simulator used for game interating.</param>
        public GameController(IWorld world, ISimulator simulator)
        {
            this.world = world 
                ?? throw new ArgumentNullException($"World is null.");
            this.simulator = simulator 
                ?? throw new ArgumentNullException($"Simulator is null.");
            seed = (bool[])this.world.State.Clone();
        }

        /// <inheritdoc/>
        public void Reset()
        {
            Buffer.BlockCopy(seed, 0, world.State, 0, Buffer.ByteLength(seed));
        }

        /// <inheritdoc/>
        public void Run(int ticks)
        {
            for (int i = 0; i < ticks; ++i)
                simulator.Tick(world);
        }
    }
}
