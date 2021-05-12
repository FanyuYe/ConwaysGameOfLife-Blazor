using Moq;
using System;
using Xunit;

namespace ConwaysGameOfLife.Core.Tests
{
    public class SimulatorTest
    {
        #region Simulator(IRule)

        [Fact]
        public void Simulator_ParameterRuleIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Simulator(null));
        }

        #endregion

        #region Tick(IWorld)

        [Fact]
        public void Tick_AllCellsBeingCovered_RuleAppliedForEachCellOnce()
        {
            const int dimension = 2;
            const int scale = 3;
            const int size = scale * scale;
            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, dimension);
            worldMock.SetupProperty(world => world.Scale, scale);
            worldMock.SetupProperty(world => world.State, new bool[size]);

            var ruleMock = new Mock<IRule>();
            ruleMock.Setup(mock => mock.GetNextIterationOfCell(It.IsAny<IWorld>(), It.IsAny<int>()));

            var world = worldMock.Object;
            var rule = ruleMock.Object;

            var sim = new Simulator(rule);
            sim.Tick(world);

            for (int i = 0; i < size; ++i)
            {
                ruleMock.Verify(mock => mock.GetNextIterationOfCell(world, i), Times.Once);
            }
        }

        [Fact]
        public void Tick_AllCellsBeingUpdatedSimultenously()
        {
            const int dimension = 1;
            const int scale = 3;
            const int size = scale;
            var worldMock = new Mock<IWorld>();
            var world = worldMock.Object;
            worldMock.SetupProperty(world => world.Dimension, dimension);
            worldMock.SetupProperty(world => world.Scale, scale);
            worldMock.SetupProperty(world => world.State, new bool[size]);

            var ruleMock = new Mock<IRule>();
            var rule = ruleMock.Object;
            // This set up indicates the cell next state is based on all other cells current value
            // Thus it only yield true if all other cells stay as false, which implies not updated yet
            ruleMock.Setup(mock => mock.GetNextIterationOfCell(It.IsAny<IWorld>(), 0))
                .Returns(!world.State[1] && !world.State[2]);
            ruleMock.Setup(mock => mock.GetNextIterationOfCell(It.IsAny<IWorld>(), 1))
                .Returns(!world.State[0] && !world.State[2]);
            ruleMock.Setup(mock => mock.GetNextIterationOfCell(It.IsAny<IWorld>(), 2))
                .Returns(!world.State[0] && !world.State[1]);

            var sim = new Simulator(rule);
            sim.Tick(world);

            for (int i = 0; i < size; ++i)
            {
                Assert.True(world.State[i]);
            }
        }

        #endregion
    }
}
