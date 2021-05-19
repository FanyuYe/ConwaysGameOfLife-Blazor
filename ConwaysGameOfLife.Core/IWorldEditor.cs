namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Abstraction for editing a world.
    /// </summary>
    internal interface IWorldEditor
    {
        /// <summary>
        /// Clear all states of the world.
        /// </summary>
        void Clear();

        /// <summary>
        /// Reset all properties describe the world back to saved values.
        /// </summary>
        void Reset();

        /// <summary>
        /// Save all properties describe the world.
        /// </summary>
        void Save();

        /// <summary>
        /// Resize the world.
        /// </summary>
        /// <param name="dimension">Dimension resize to.</param>
        /// <param name="scale">Scale resize to.</param>
        void Resize(int dimension, int scale);

        /// <summary>
        /// Set state of a cell in the world.
        /// </summary>
        /// <param name="coordinate">Coordinate of the cell.</param>
        /// <param name="newState">New state of the cell.</param>
        void SetState(int coordinate, bool newState);
    }
}
