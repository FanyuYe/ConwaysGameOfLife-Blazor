namespace ConwaysGameOfLife.Core
{
    public interface IAlgorithm
    {
        public void ComputeNextIteration(bool[] currentState, out bool[] nextState);
    }
}
