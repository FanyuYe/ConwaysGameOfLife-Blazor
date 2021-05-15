﻿using Moq;
using System;
using Xunit;

namespace ConwaysGameOfLife.Core.Tests
{
    /// <summary>
    /// TODO: 
    /// 1. One Assert per test.
    /// </summary>
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
            var ruleMock = new Mock<IRule>();
            ruleMock.Setup(mock => mock.GetNextIterationOfCell(It.IsAny<IWorld>(), It.IsAny<int>()));
            var rule = ruleMock.Object;
            var world = TestHelper.CreateMockWorld2D_3x3();
            
            var sim = new Simulator(rule);
            sim.Tick(world);

            for (int i = 0; i < 9; ++i)
            {
                ruleMock.Verify(mock => mock.GetNextIterationOfCell(world, i), Times.Once);
            }
        }

        [Fact]
        public void Tick_AllCellsBeingUpdatedSimultenously()
        {
            var world = TestHelper.CreateMockWorld1D_3();
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

            for (int i = 0; i < 3; ++i)
            {
                Assert.True(world.State[i]);
            }
        }

        #endregion
    }
}
