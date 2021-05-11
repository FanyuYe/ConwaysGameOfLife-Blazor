namespace ConwaysGameOfLife.Core
{
    public interface IGameController
    {
        /// <summary>
        /// Reset the game.
        /// </summary>
        public void Reset();

        /// <summary>
        /// Run the game for specified ticks.
        /// </summary>
        /// <param name="steps"></param>
        public void Run(int ticks);
    }
}
