namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Provide a way to make rules being quantitatively configurable.
    /// </summary>
    public interface IRuleConfigurable
    {
        /// <summary>
        /// The minimum number of neighbours for the cell to survive the iteration. 
        /// </summary>
        public int UnderpopulationThreshold { get; set; }

        /// <summary>
        /// The maximum number of neighbours for the cell to survice the iteration.
        /// </summary>
        public int OverpopulationThreshold { get; set; }

        /// <summary>
        /// The exact number of neighbours for the empty cell to spwan the life.
        /// </summary>
        public int ReproductionNeighbourCount { get; set; }
    }
}
