using System;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Implement rules to run classic conway's game of life.
    /// Threshold of rules are all configurable.
    /// </summary>
    internal class Rule : IRule
    {
        private readonly IRuleConfigurable _config;
        private readonly IWorldInterpreter _worldInterpreter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">Configuration used by the rule. <seealso cref="IRuleConfigurable"/></param>
        /// <param name="worldInterpreter">World interpreter used to query the world. <seealso cref="IWorldInterpreter"/></param>
        public Rule(IRuleConfigurable config, IWorldInterpreter worldInterpreter)
        {
            _config = config
                ?? throw new ArgumentNullException(nameof(config));
            _worldInterpreter = worldInterpreter
                ?? throw new ArgumentNullException(nameof(worldInterpreter));
        }

        /// <inheritdoc/>
        public bool GetNextIterationOfCell(IWorld world, int cellCoordinate)
        {
            bool currentState = world.State[cellCoordinate];
            int count = 0;

            foreach(bool b in _worldInterpreter.GetNeighbourStatesFromCell(world, cellCoordinate))
            {
                count += b ? 1 : 0;

                if (count > _config.OverpopulationThreshold)
                    return false;
            }

            if (count < _config.UnderpopulationThreshold)
                return false;

            if (!currentState && count == _config.ReproductionNeighbourCount)
                return true;

            return currentState;
        }
    }
}
