using System;
using System.Linq;

namespace ConwaysGameOfLife.Core
{
    /// <summary>
    /// Editor of conway's game of life.
    /// </summary>
    internal class WorldEditor : IWorldEditor
    {
        private readonly IWorld _world;
        private readonly ICoordinateConverter _converter;
        private readonly IWorld _savedWorld;

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="world">World the editor is working on.</param>
        public WorldEditor(IWorld world, ICoordinateConverter converter)
        {
            _world = world
                ?? throw new ArgumentNullException(nameof(world));
            _converter = converter
                ?? throw new ArgumentNullException(nameof(converter));

            _savedWorld = new World(_world.Dimension, _world.Scale, _world.State);
        }

        #endregion

        #region IWorldEditor

        /// <inheritdoc/>
        public void Clear()
        {
            for (int i = 0; i < _world.State.Length; ++i)
                _world.State[i] = false;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            (_world.Dimension, _world.Scale) = (_savedWorld.Dimension, _savedWorld.Scale);
            Buffer.BlockCopy(_savedWorld.State, 0, _world.State, 0, Buffer.ByteLength(_savedWorld.State));
        }

        /// <inheritdoc/>
        public void Save()
        {
            (_savedWorld.Dimension, _savedWorld.Scale) = (_world.Dimension, _world.Scale);
            Buffer.BlockCopy(_world.State, 0, _savedWorld.State, 0, Buffer.ByteLength(_world.State));
        }

        /// <inheritdoc/>
        public void Resize(int dimension, int scale)
        {
            if (dimension <= 0)
                throw new ArgumentOutOfRangeException(nameof(dimension), $"Dimension should be a positive integer.");

            if (scale <= 0)
                throw new ArgumentOutOfRangeException(nameof(scale), $"Scale should be a positive integer.");

            if (_world.Dimension == dimension && _world.Scale == scale)
                return;

            int oldDimension = _world.Dimension;
            int oldScale = _world.Scale;
            bool[] oldState = new bool[_world.State.Length];
            Buffer.BlockCopy(_world.State, 0, oldState, 0, Buffer.ByteLength(_world.State));
            int newLength = (int)Math.Pow(scale, dimension);
            bool[] newState = new bool[newLength];
            int[] oldCoo = new int[oldDimension];
            int delta = dimension - oldDimension;

            for (int i = 0; i < newLength; ++i)
            {
                int[] newCoo = _converter.ConvertCoordinateSingleToMulti(dimension, scale, i);

                // Clamp dimension
                if (delta > 0 && Enumerable.Range(oldDimension, delta).Any(x => newCoo[x] != 0))
                    continue;

                for (int j = 0; j < oldDimension; ++j)
                {
                    oldCoo[j] = (j < dimension) ? newCoo[j] : 0;
                }

                // Clamp scale
                if (!oldCoo.All(x => x < oldScale))
                    continue;

                newState[i] = oldState[_converter.ConvertCoordinateMultiToSingle(oldScale, oldCoo)];
            }

            _world.Dimension = dimension;
            _world.Scale = scale;
            _world.State = newState;
        }

        /// <inheritdoc/>
        public void SetState(int coordinate, bool newState) => _world.State[coordinate] = newState;

        #endregion
    }
}
