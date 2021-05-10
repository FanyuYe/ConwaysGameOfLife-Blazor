namespace ConwaysGameOfLife.Core
{
    public interface IAlgorithm
    {
        public void ComputeNextIteration(int dimension, int scale, bool[] currentState, out bool[] nextState);
    }
}
