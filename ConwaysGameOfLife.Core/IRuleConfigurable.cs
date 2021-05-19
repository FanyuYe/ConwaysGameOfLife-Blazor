namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Provide a way to make rules being quantitatively configurable.
    /// </summary>
    internal interface IRuleConfigurable
    {
        /// <summary>
        /// The minimum number of neighbours for the cell to survive the iteration. 
        /// </summary>
        int UnderpopulationThreshold { get; set; }

        /// <summary>
        /// The maximum number of neighbours for the cell to survice the iteration.
        /// </summary>
        int OverpopulationThreshold { get; set; }

        /// <summary>
        /// The exact number of neighbours for the empty cell to spwan the life.
        /// </summary>
        int ReproductionNeighbourCount { get; set; }
    }
}
