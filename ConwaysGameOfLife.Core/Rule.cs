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

        /// <summary>
        /// Constructor..
        /// </summary>
        /// <param name="config">Configuration used by the rule. <seealso cref="IRuleConfigurable"/></param>
        public Rule(IRuleConfigurable config)
        {
            if (config == null)
                throw new ArgumentNullException($"Config is null.");

            this.config = config;
        }

        /// <inheritdoc/>
        public bool GetNextIterationOfCell(IWorld world, int cellCoordinate)
        {
            bool currentState = world.State[cellCoordinate];
            int count = 0;

            foreach(bool b in world.GetNeighbourStatesFromCell(cellCoordinate))
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
