using System;
using System.Threading.Tasks;

namespace ConwaysGameOfLife.Core
{
    public class Simulator : ISimulator
    {
        private readonly IRule rule;

        public Simulator(IRule rule)
        {
            if (rule == null)
                throw new ArgumentNullException($"Rule is null.");

            this.rule = rule;
        }

        public void Tick(IWorld world)
        {
            var nextState = new bool[world.State.Length];

            Parallel.For(0, nextState.Length, i => 
            {
                nextState[i] = rule.GetNextIterationOfCell(world, i);
            });

            Buffer.BlockCopy(nextState, 0, world.State, 0, Buffer.ByteLength(nextState));
        }
    }
}
