using System;

namespace ConwaysGameOfLife.Core
{
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

        #endregion

        #region API

        public int Scale
        {
            get => _viewer.Scale;

            set => _editor.Resize(_DIMENSION, value);
        }

        public bool GetState(int x, int y)
        {
            return _viewer.GetState(
                _converter.ConvertCoordinateMultiToSingle(
                    _viewer.Scale, new int[_DIMENSION] { x, y }));
        }

        public void SetState(int x, int y, bool newState)
        {
            _editor.SetState(
                _converter.ConvertCoordinateMultiToSingle(
                    _viewer.Scale, new int[_DIMENSION] { x, y }), newState);
        }

        public void Reset() => _editor.Reset();

        public void Clear() => _editor.Clear();

        public void Save() => _editor.Save();

        public void Run() => _simulator.Tick();

        #endregion
    }
}
