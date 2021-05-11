using System;

namespace ConwaysGameOfLife.Core
{
    public class Simulator : ISimulator
    {
        private IRule rule;

        public Simulator(IRule rule)
        {
            if (rule == null)
                throw new ArgumentNullException($"Rule is null.");

            this.rule = rule;
        }

        public void Tick(IWorld world)
        {
            throw new NotImplementedException();
        }
    }
}
