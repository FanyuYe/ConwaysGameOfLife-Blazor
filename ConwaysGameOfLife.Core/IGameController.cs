namespace ConwaysGameOfLife.Core
{
    public interface IGameController
    {
        /// <summary>
        /// Reset the game.
        /// </summary>
        void Reset();

        /// <summary>
        /// Run the game for specified ticks.
        /// </summary>
        /// <param name="steps"></param>
        void Run(int ticks);
    }
}
