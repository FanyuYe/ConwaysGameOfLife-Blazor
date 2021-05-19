using System;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// 2D implmentation of conway's game of life.
    /// </summary>
    public class ConwaysGameOfLife2D
    {
        #region Members

        private const int _DIMENSION = 2;

        private readonly IWorldViewer _viewer;
        private readonly IWorldEditor _editor;
        private readonly ISimulator _simulator;
        private readonly ICoordinateConverter _converter;

        #endregion

        #region Constructor / Factory Method

        internal ConwaysGameOfLife2D(IWorldViewer viewer, IWorldEditor editor, ISimulator simulator, ICoordinateConverter converter)
        {
            _viewer = viewer ?? throw new ArgumentNullException(nameof(viewer));
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
            _simulator = simulator ?? throw new ArgumentNullException(nameof(simulator));
            _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        #endregion

        #region API

        /// <summary>
        /// Create a classic game with certain scale.
        /// </summary>
        /// <param name="scale">Scale for the game.</param>
        /// <returns>Game object.</returns>
        public static ConwaysGameOfLife2D CreateClassicGame(int scale)
        {
            var converter = new CoordinateConverter();

            var world = new World(_DIMENSION, scale);

            var viewer = new WorldViewer(world);

            var editor = new WorldEditor(world, converter);

            var simulator = new Simulator(world,
                new Rule(RuleConfiguration.Default,
                    new WorldInterpreter(
                        new CoordinateConverter())));

            return new ConwaysGameOfLife2D(viewer, editor, simulator, converter);
        }

        /// <summary>
        /// Scale of the game world.
        /// </summary>
        public int Scale
        {
            get => _viewer.Scale;

            set => _editor.Resize(_DIMENSION, value);
        }

        /// <summary>
        /// Get state of a cell by coordinate.
        /// </summary>
        /// <param name="x">X axis coordinate.</param>
        /// <param name="y">Y axis coordinate.</param>
        /// <returns>State of the cell.</returns>
        public bool GetState(int x, int y)
        {
            return _viewer.GetState(
                _converter.ConvertCoordinateMultiToSingle(
                    _viewer.Scale, new int[_DIMENSION] { x, y }));
        }

        /// <summary>
        /// Set state of a cell by coordinate.
        /// </summary>
        /// <param name="x">X axis coordinate.</param>
        /// <param name="y">Y axis coordinate.</param>
        /// <param name="newState">New state of the cell set to.</param>
        public void SetState(int x, int y, bool newState)
        {
            _editor.SetState(
                _converter.ConvertCoordinateMultiToSingle(
                    _viewer.Scale, new int[_DIMENSION] { x, y }), newState);
        }

        /// <summary>
        /// Reset game back to previous save.
        /// </summary>
        public void Reset() => _editor.Reset();

        /// <summary>
        /// Clear game world. All cell back to empty.
        /// </summary>
        public void Clear() => _editor.Clear();

        /// <summary>
        /// Save game.
        /// </summary>
        public void Save() => _editor.Save();

        /// <summary>
        /// Run game for a single interation.
        /// </summary>
        public void Run() => _simulator.Tick();

        #endregion
    }
}
