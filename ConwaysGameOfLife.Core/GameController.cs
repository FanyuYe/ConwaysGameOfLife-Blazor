using System;

namespace ConwaysGameOfLife.Core
{
    public class GameController : IGameController
    {
        private IWorld world;
        private ISimulator simulator;

        public GameController(IWorld world, ISimulator simulator)
        {
            if (world == null)
                throw new ArgumentNullException($"World is null.");

            if (simulator == null)
                throw new ArgumentNullException($"Simulator is null.");

            this.world = world;
            this.simulator = simulator;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Run(int ticks)
        {
            throw new NotImplementedException();
        }
    }
}
