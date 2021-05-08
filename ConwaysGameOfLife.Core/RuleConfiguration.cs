namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Configuration of rules for the conway's game of life.
    /// </summary>
    public class RuleConfiguration
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

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="underpopulationThreshold">The minimum number of neighbours for the cell to survive the iteration.</param>
        /// <param name="overpopulationThreshold">The maximum number of neighbours for the cell to survice the iteration.</param>
        /// <param name="reproductionNeighbourCount">The exact number of neighbours for the empty cell to spwan the life.</param>
        public RuleConfiguration(int underpopulationThreshold, int overpopulationThreshold, int reproductionNeighbourCount) =>
            (UnderpopulationThreshold, OverpopulationThreshold, ReproductionNeighbourCount) = 
            (underpopulationThreshold, overpopulationThreshold, reproductionNeighbourCount);

        /// <summary>
        /// Default configuration in a classic conway's game of life.
        /// <para><seealso cref="UnderpopulationThreshold"/> = 2</para>
        /// <para><seealso cref="OverpopulationThreshold"/> = 3</para>
        /// <para><seealso cref="ReproductionNeighbourCount"/> = 3</para>
        /// </summary>
        public static RuleConfiguration Default => new RuleConfiguration(2, 3, 3);
    }
}
