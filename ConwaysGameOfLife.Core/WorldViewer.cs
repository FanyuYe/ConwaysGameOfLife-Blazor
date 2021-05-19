using System;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Viewer of a conway's game of life.
    /// </summary>
    internal class WorldViewer : IWorldViewer
    {
        private readonly IWorld _world;

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="world">World to view.</param>
        public WorldViewer(IWorld world)
        {
            _world = world
                ?? throw new ArgumentNullException(nameof(world));
        }

        #endregion

        #region IWorldViewer

        /// <inheritdoc/>
        public int Dimension => _world.Dimension;

        /// <inheritdoc/>
        public int Scale => _world.Scale;

        /// <inheritdoc/>
        public bool GetState(int cooridnate) => _world.State[cooridnate];

        #endregion
    }
}
