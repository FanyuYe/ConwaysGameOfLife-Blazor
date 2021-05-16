using System;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Implement rules to run classic conway's game of life.
    /// Threshold of rules are all configurable.
    /// </summary>
    public class Rule : IRule
    {
        private readonly IRuleConfigurable config;
        private readonly IWorldInterpreter worldInterpreter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">Configuration used by the rule. <seealso cref="IRuleConfigurable"/></param>
        /// <param name="worldInterpreter">World interpreter used to query the world. <seealso cref="IWorldInterpreter"/></param>
        public Rule(IRuleConfigurable config, IWorldInterpreter worldInterpreter)
        {
            this.config = config
                ?? throw new ArgumentNullException(nameof(config));
            this.worldInterpreter = worldInterpreter
                ?? throw new ArgumentNullException(nameof(worldInterpreter));
        }

        /// <inheritdoc/>
        public bool GetNextIterationOfCell(IWorld world, int cellCoordinate)
        {
            bool currentState = world.State[cellCoordinate];
            int count = 0;

            foreach(bool b in worldInterpreter.GetNeighbourStatesFromCell(world, cellCoordinate))
            {
                count += b ? 1 : 0;

                if (count > config.OverpopulationThreshold)
                    return false;
            }

            if (count < config.UnderpopulationThreshold)
                return false;

            if (!currentState && count == config.ReproductionNeighbourCount)
                return true;

            return currentState;
        }
    }
}
