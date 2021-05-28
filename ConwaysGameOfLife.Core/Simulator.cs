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
        private readonly bool[] _nextState;

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

            _nextState = new bool[_world.State.Length];
        }

        /// <inheritdoc/>
        public void Tick()
        {
            Parallel.For(0, _nextState.Length, i => 
            {
                _nextState[i] = _rule.GetNextIterationOfCell(_world, i);
            });

            Buffer.BlockCopy(_nextState, 0, _world.State, 0, Buffer.ByteLength(_nextState));
        }
    }
}
