using System;
using System.Threading.Tasks;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Simulate conways's game of life.
    /// </summary>
    internal class Simulator : ISimulator
    {
        private readonly IWorld _world;
        private readonly IRule _rule;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rule">Rule used for simulating.</param>
        public Simulator(IWorld world, IRule rule)
        {
            _world = world
                ?? throw new ArgumentNullException(nameof(rule));

            _rule = rule
                ?? throw new ArgumentNullException(nameof(rule));
        }

        /// <inheritdoc/>
        public void Tick()
        {
            var nextState = new bool[_world.State.Length];

            Parallel.For(0, nextState.Length, i => 
            {
                nextState[i] = _rule.GetNextIterationOfCell(_world, i);
            });

            Buffer.BlockCopy(nextState, 0, _world.State, 0, Buffer.ByteLength(nextState));
        }
    }
}
