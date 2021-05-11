using System;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Implement rules to run classic conway's game of life.
    /// Threshold of rules are all configurable.
    /// </summary>
    public class Rule : IRule
    {
        private IRuleConfigurable config;

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
        public bool ComputeNextIterationOfCell(IWorld world, int cellCoordinate)
        {
            throw new NotImplementedException();
        }
    }
}
