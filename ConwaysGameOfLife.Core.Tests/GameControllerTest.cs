using Moq;
using System;
using Xunit;

namespace ConwaysGameOfLife.Core.Tests
{
    public class GameControllerTest
    {
        #region GameController(IWorld world, ISimulator simulator)

        [Fact]
        public void GameController_ParameterWorldIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GameController(null, Mock.Of<ISimulator>()));
        }

        [Fact]
        public void GameController_ParameterSimulatorIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GameController(Mock.Of<IWorld>(), null));
        }

        #endregion

        #region Reset()
        
        [Fact]
        public void Reset_InvokeAfterModifyWorldState_WorldStateEqualToOriginState()
        {
            var expected = new bool[9];
            var world = TestHelper.CreateMockWorld2D_3x3();
            var sim = Mock.Of<ISimulator>();
            var gameController = new GameController(world, sim);

            for (int i = 0; i < world.State.Length; ++i)
                world.State[i] = true;
            gameController.Reset();

            Assert.Equal<bool>(expected, world.State);
        }

        #endregion

        #region Run(int)

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        [InlineData(-1000)]
        public void Run_InvokeWithZeroOrNegativeTicksParameter_SimulatorNeverRuns(int ticks)
        {
            var world = Mock.Of<IWorld>();
            var simMock = new Mock<ISimulator>();
            var gameController = new GameController(world, simMock.Object);
            simMock.Setup(sim => sim.Tick(world));

            gameController.Run(ticks);

            simMock.Verify(sim => sim.Tick(world), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void Run_InvokeWithPositiveTicksParameter_SimulatorRunsSameAmountOfTimes(int ticks)
        {
            var world = Mock.Of<IWorld>();
            var simMock = new Mock<ISimulator>();
            var gameController = new GameController(world, simMock.Object);
            simMock.Setup(sim => sim.Tick(world));

            gameController.Run(ticks);

            simMock.Verify(sim => sim.Tick(world), Times.Exactly(ticks));
        }
        
        #endregion
    }
}
