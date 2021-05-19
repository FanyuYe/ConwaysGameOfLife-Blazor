using Moq;
using System;
using System.Linq;
using Xunit;

namespace ConwaysGameOfLife.Core.Tests
{
    public class WorldEditorTest
    {
        #region WorldEditor(IWorld world)

        [Fact]
        public void WorldEditor_ParameterWorldIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WorldEditor(null, Mock.Of<ICoordinateConverter>()));
        }

        [Fact]
        public void WorldEditor_ParameterCoordinateConverterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WorldEditor(Mock.Of<IWorld>(), null));
        }

        [Fact]
        public void WorldEditor_CallResetStraightAway_SavedWorldInstantiatedThusNotThrowsAnyException()
        {
            var world = TestHelper.CreateMockWorld2D_3x3();
            var editor = new WorldEditor(world, Mock.Of<ICoordinateConverter>());

            editor.Reset();

            // No need assert here
        }

        #endregion

        #region Clear()

        [Fact]
        public void Clear_World2D_3x3_DimensionNotChanged()
        {
            var world = TestHelper.CreateMockWorld2D_3x3();
            var editor = new WorldEditor(world, Mock.Of<ICoordinateConverter>());
            int expected = world.Dimension;

            editor.Clear();

            Assert.Equal(expected, world.Dimension);
        }

        [Fact]
        public void Clear_World2D_3x3_ScaleNotChanged()
        {
            var world = TestHelper.CreateMockWorld2D_3x3();
            var editor = new WorldEditor(world, Mock.Of<ICoordinateConverter>());
            int expected = world.Scale;

            editor.Clear();

            Assert.Equal(expected, world.Scale);
        }

        [Fact]
        public void Clear_World2D_3x3_AllTrue_AllWorldStatesAreFalse()
        {
            var world = TestHelper.CreateMockWorld2D_3x3(Enumerable.Repeat(true, 9).ToArray());
            var editor = new WorldEditor(world, Mock.Of<ICoordinateConverter>());
            int expected = world.Scale;

            editor.Clear();

            Assert.All(world.State, item => Assert.False(item));
        }

        #endregion

        #region Save() + Reset()

        [Fact]
        public void Save_Reset_ChangeDimensionToFour_Save_ChangeDimensionToFive_Reset_DimensionBackToFour()
        {
            var world = TestHelper.CreateMockWorld2D_3x3();
            var editor = new WorldEditor(world, Mock.Of<ICoordinateConverter>());
            var expected = 4;

            world.Dimension = expected;
            editor.Save();
            world.Dimension = 5;
            editor.Reset();

            Assert.Equal(expected, world.Dimension);
        }

        [Fact]
        public void Save_Reset_ChangeScaleToFour_Save_ChangeScaleToFive_Reset_ScaleBackToFour()
        {
            var world = TestHelper.CreateMockWorld2D_3x3();
            var editor = new WorldEditor(world, Mock.Of<ICoordinateConverter>());
            var expected = 4;

            world.Scale = expected;
            editor.Save();
            world.Scale = 5;
            editor.Reset();

            Assert.Equal(expected, world.Scale);
        }

        [Fact]
        public void Save_Reset_SaveThenSetStateThenReset_WorldStateEqualToSavedState()
        {
            var world = TestHelper.CreateMockWorld2D_3x3();
            var editor = new WorldEditor(world, Mock.Of<ICoordinateConverter>());
            var expected = (bool[])world.State.Clone();

            editor.Save();
            for (int i = 0; i < world.State.Length; ++i)
                world.State[i] = true;
            editor.Reset();

            Assert.Equal(expected, world.State);
        }

        #endregion

        #region Resize(int, int)

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Resize_ParameterScaleIsNonPositive_ThrowsArgumentOutOfRangeException(int scale)
        {
            var editor = new WorldEditor(TestHelper.CreateMockWorld2D_3x3(), Mock.Of<ICoordinateConverter>());

            Assert.Throws<ArgumentOutOfRangeException>(() => editor.Resize(It.IsAny<int>(), scale));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Resize_ParameterDimensionIsNonPositive_ThrowsArgumentOutOfRangeException(int dimension)
        {
            var editor = new WorldEditor(TestHelper.CreateMockWorld2D_3x3(), TestHelper.CreateMockCoordinateConverter(1));

            Assert.Throws<ArgumentOutOfRangeException>(() => editor.Resize(dimension, It.IsAny<int>()));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 3)]
        [InlineData(2, 2)]
        [InlineData(2, 4)]
        [InlineData(3, 3)]
        public void Resize_WorldDimensionAndScaleUpdatedToProvidedValues(int dimension, int scale)
        {
            var world = TestHelper.CreateMockWorld2D_3x3();
            var editor = new WorldEditor(world, TestHelper.CreateMockCoordinateConverter());

            editor.Resize(dimension, scale);

            Assert.True(world.Dimension == dimension && world.Scale == scale);
        }

        [Theory]
        [InlineData(1, 3, new int[3] { 0, 1, 2 })]
        [InlineData(2, 2, new int[4] { 0, 1, 2, 3 })]
        [InlineData(2, 4, new int[9] { 0, 1, 2, 4, 5, 6, 8, 9, 10 })]
        [InlineData(3, 3, new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 })]
        public void Resize_OnWorld2D_3x3_AllPreservedCellsAreTrue(int dimension, int scale, int[] preservedCells)
        {
            var world = TestHelper.CreateMockWorld2D_3x3(Enumerable.Repeat(true, 9).ToArray());
            var editor = new WorldEditor(world, TestHelper.CreateMockCoordinateConverter());

            editor.Resize(dimension, scale);

            Assert.All(preservedCells, coo => Assert.True(world.State[coo]));
        }

        [Theory]
        [InlineData(1, 3, new int[0])]
        [InlineData(2, 2, new int[0])]
        [InlineData(2, 4, new int[7] { 3, 7, 11, 12, 13, 14, 15 })]
        [InlineData(3, 3, new int[18] { 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 })]
        public void Resize_OnWorld2D_3x3_AllNewCellsAreFalse(int dimension, int scale, int[] newCells)
        {
            var world = TestHelper.CreateMockWorld2D_3x3(Enumerable.Repeat(true, 9).ToArray());
            var editor = new WorldEditor(world, TestHelper.CreateMockCoordinateConverter());

            editor.Resize(dimension, scale);

            Assert.All(newCells, coo => Assert.False(world.State[coo]));
        }

        #endregion

        #region SetState(int, bool)

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        [InlineData(5, true)]
        [InlineData(6, true)]
        [InlineData(7, true)]
        [InlineData(8, true)]
        public void SetState_World2D_3x3_AllFalse_SetToTrue_CellStateIsTrue(int coordinate, bool newState)
        {
            var world = TestHelper.CreateMockWorld2D_3x3();
            var editor = new WorldEditor(world, Mock.Of<ICoordinateConverter>());

            editor.SetState(coordinate, newState);

            Assert.Equal(newState, world.State[coordinate]);
        }

        #endregion
    }
}
