using Moq;
using System;
using System.Linq;
using Xunit;

namespace ConwaysGameOfLife.Core.Tests
{
    public class RuleTest
    {
        #region Init

        private IRuleConfigurable CreateMockRuleConfiguration()
        {
            const int underpopulationThreshold = 2;
            const int overpopulationThreshold = 3;
            const int reproductionNeighbourCount = 3;
            var ruleConfigMock = new Mock<IRuleConfigurable>()
                .SetupProperty(cfg => cfg.UnderpopulationThreshold, underpopulationThreshold)
                .SetupProperty(cfg => cfg.OverpopulationThreshold, overpopulationThreshold)
                .SetupProperty(cfg => cfg.ReproductionNeighbourCount, reproductionNeighbourCount);

            return ruleConfigMock.Object;
        }

        private IWorldInterpreter CreateWorldInterpreterMock(int length, int trueCount)
        {
            var trueNeighbour = Enumerable.Repeat(true, trueCount);
            var falseNeighbour = Enumerable.Repeat(false, length - trueCount);
            var worldInterpreterMock = new Mock<IWorldInterpreter>();
            worldInterpreterMock.Setup(wim => wim.GetNeighbourStatesFromCell(It.IsAny<IWorld>(), It.IsAny<int>()))
                .Returns(trueNeighbour.Concat(falseNeighbour));

            return worldInterpreterMock.Object;
        }

        #endregion

        #region Rule(IRuleConfigurable)

        [Fact]
        public void Constructor_ConfigParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Rule(null, Mock.Of<IWorldInterpreter>()));
        }

        [Fact]
        public void Constructor_WorldInterpreterParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Rule(Mock.Of<IRuleConfigurable>(), null));
        }

        #endregion

        #region GetNextIterationOfCell(IWorld, int)

        [Theory]
        [InlineData(0, 1)] // alive cell, 1 neighbour
        [InlineData(2, 1)] // empty cell, 1 neighbour
        [InlineData(6, 0)] // alive cell, 0 neighbour
        [InlineData(8, 0)] // empty cell, 0 neighbour
        public void GetNextIterationOfCell_AliveOrEmptyCell_NeighbourLessThanUnderpopulationThreshold_ReturnFalse(
            int targetCell, int neighbourCount)
        {
            var world = TestHelper.CreateMockWorld2D_3x3(new bool[9]
                {
                    true,  true,  false,
                    false, false, false,
                    true,  false, false
                });
            var rule = new Rule(
                CreateMockRuleConfiguration(), 
                CreateWorldInterpreterMock(9, neighbourCount));

            bool actual = rule.GetNextIterationOfCell(world, targetCell);

            bool expected = false;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 4)] // alive cell, 4 neighbour
        [InlineData(7, 5)] // empty cell, 5 neighbour
        public void GetNextIterationOfCell_AliveOrEmptyCell_NeighbourGreaterThanOverpopulationThreshold_ReturnFalse(
            int targetCell, int neighbourCount)
        {
            var world = TestHelper.CreateMockWorld2D_3x3(new bool[9]
                {
                    true, true,  false,
                    true, true,  true,
                    true, false, true
                });
            var rule = new Rule(
                CreateMockRuleConfiguration(),
                CreateWorldInterpreterMock(9, neighbourCount));

            bool actual = rule.GetNextIterationOfCell(world, targetCell);

            bool expected = false;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 2)] // alive cell, 2 neighbour
        [InlineData(8, 3)] // alive cell, 3 neighbour
        public void GetNextIterationOfCell_AliveCell_NeighbourBetweenUnderpopulationAndOverpopulationThreshold_ReturnTrue(
            int targetCell, int neighbourCount)
        {
            var world = TestHelper.CreateMockWorld2D_3x3(new bool[9]
                {
                    true,  false, false,
                    true,  true,  true,
                    false, true,  true
                });
            var rule = new Rule(
                CreateMockRuleConfiguration(),
                CreateWorldInterpreterMock(9, neighbourCount));

            bool actual = rule.GetNextIterationOfCell(world, targetCell);

            bool expected = true;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3)] // empty cell, 3 neighbour
        public void GetNextIterationOfCell_EmptyCell_NeighbourEqualToReproductionThreshold_ReturnTrue(
            int targetCell, int neighbourCount)
        {
            var world = TestHelper.CreateMockWorld2D_3x3(new bool[9]
                {
                    false, true,  false,
                    true,  true,  false,
                    false, false, false
                });
            var rule = new Rule(
                CreateMockRuleConfiguration(),
                CreateWorldInterpreterMock(9, neighbourCount));

            bool actual = rule.GetNextIterationOfCell(world, targetCell);

            bool expected = true;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 2)] // empty cell, 2 neighbour
        [InlineData(1, 4)] // empty cell, 4 neighbour
        [InlineData(7, 5)] // empty cell, 5 neighbour
        public void GetNextIterationOfCell_EmptyCell_NeighbourNotEqualToReproductionThreshold_ReturnFalse(
            int targetCell, int neighbourCount)
        {
            var world = TestHelper.CreateMockWorld2D_3x3(new bool[9]
                {
                    false, false, true,
                    true,  true,  true,
                    true,  false, true
                });
            var rule = new Rule(
                 CreateMockRuleConfiguration(),
                 CreateWorldInterpreterMock(9, neighbourCount));

            bool actual = rule.GetNextIterationOfCell(world, targetCell);

            bool expected = false;
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
