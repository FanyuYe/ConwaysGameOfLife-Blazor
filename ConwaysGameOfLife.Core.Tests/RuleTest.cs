using Moq;
using System;
using Xunit;

namespace ConwaysGameOfLife.Core.Tests
{
    /// <summary>
    /// TODO: 
    /// 1. One Assert per test.
    /// </summary>
    public class RuleTest
    {
        #region Init

        private readonly Rule rule;

        public RuleTest()
        {
            rule = new Rule(CreateMockRuleConfiguration());
        }

        private IRuleConfigurable CreateMockRuleConfiguration()
        {
            const int underpopulationThreshold = 2;
            const int overpopulationThreshold = 3;
            const int reproductionNeighbourCount = 3;
            var ruleConfigMock = new Mock<IRuleConfigurable>();
            ruleConfigMock.SetupProperty(cfg => cfg.UnderpopulationThreshold, underpopulationThreshold);
            ruleConfigMock.SetupProperty(cfg => cfg.OverpopulationThreshold, overpopulationThreshold);
            ruleConfigMock.SetupProperty(cfg => cfg.ReproductionNeighbourCount, reproductionNeighbourCount);

            return ruleConfigMock.Object;
        }

        private IWorld CreateMockWorld2D_3x3()
        {
            const int dimension = 2;
            const int scale = 3;
            const int size = scale * scale;
            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, dimension);
            worldMock.SetupProperty(world => world.Scale, scale);
            worldMock.SetupProperty(world => world.State, new bool[size]);

            return worldMock.Object;
        }

        #endregion

        #region Rule(IRuleConfigurable)

        [Fact]
        public void Constructor_ConfigParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Rule(null));
        }

        #endregion

        #region GetNextIterationOfCell(IWorld, int)

        [Fact]
        public void GetNextIterationOfCell_AliveOrEmptyCell_NeighbourLessThanUnderpopulationThreshold_ReturnFalse()
        {
            var world = CreateMockWorld2D_3x3();
            world.State = new bool[9]
                {
                    true,  true,  false,
                    false, false, false,
                    true,  false, false
                };

            int aliveOneNeighbour = 0;
            int aliveZeroNeighbour = 6;
            int emptyOneNeighbour = 2;
            int emptyZeroNeighbour = 8;

            bool expected = false;
            bool actualAliveOneNeighbour = rule.GetNextIterationOfCell(world, aliveOneNeighbour);
            bool actualAliveZeroNeighbour = rule.GetNextIterationOfCell(world, aliveZeroNeighbour);
            bool actualEmptyOneNeighbour = rule.GetNextIterationOfCell(world, emptyOneNeighbour);
            bool actualEmptyZeroNeighbour = rule.GetNextIterationOfCell(world, emptyZeroNeighbour);

            Assert.Equal(expected, actualAliveOneNeighbour);
            Assert.Equal(expected, actualAliveZeroNeighbour);
            Assert.Equal(expected, actualEmptyOneNeighbour);
            Assert.Equal(expected, actualEmptyZeroNeighbour);
        }

        [Fact]
        public void GetNextIterationOfCell_AliveOrEmptyCell_NeighbourGreaterThanOverpopulationThreshold_ReturnFalse()
        {
            var world = CreateMockWorld2D_3x3();
            world.State = new bool[9]
                {
                    true, true,  false,
                    true, true,  true,
                    true, false, true
                };

            int aliveFourNeighbour = 1;
            int emptyFiveNeighbour = 7;
            bool actualAliveFourNeighbour = rule.GetNextIterationOfCell(world, aliveFourNeighbour);
            bool actualEmptyFiveNeighbour = rule.GetNextIterationOfCell(world, emptyFiveNeighbour);

            bool expected = false;
            Assert.Equal(expected, actualAliveFourNeighbour);
            Assert.Equal(expected, actualEmptyFiveNeighbour);
        }

        [Fact]
        public void GetNextIterationOfCell_AliveCell_NeighbourBetweenUnderpopulationAndOverpopulationThreshold_ReturnTrue()
        {
            var world = CreateMockWorld2D_3x3();
            world.State = new bool[9]
                {
                    true,  false, false,
                    true,  true,  true,
                    false, true,  true
                };

            int twoNeighbour = 0;
            int threeNeighbour = 8;
            bool actualTwoNeighbour = rule.GetNextIterationOfCell(world, twoNeighbour);
            bool actualThreeNeighbour = rule.GetNextIterationOfCell(world, threeNeighbour);

            bool expected = true;
            Assert.Equal(expected, actualTwoNeighbour);
            Assert.Equal(expected, actualThreeNeighbour);
        }

        [Fact]
        public void GetNextIterationOfCell_EmptyCell_NeighbourEqualToReproductionThreshold_ReturnTrue()
        {
            var world = CreateMockWorld2D_3x3();
            world.State = new bool[9]
                {
                    false, true,  false,
                    true,  true,  false,
                    false, false, false
                };

            int threeNeighbour = 0;
            bool actual = rule.GetNextIterationOfCell(world, threeNeighbour);

            bool expected = true;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNextIterationOfCell_EmptyCell_NeighbourNotEqualToReproductionThreshold_ReturnFalse()
        {
            var world = CreateMockWorld2D_3x3();
            world.State = new bool[9]
                {
                    false, false, true,
                    true,  true,  true,
                    true,  false, true
                };

            int twoNeighbour = 0;
            int fourNeighbour = 1;
            int fiveNeighbour = 7;
            bool actualTwoNeighbour = rule.GetNextIterationOfCell(world, twoNeighbour);
            bool actualFourNeighbour = rule.GetNextIterationOfCell(world, fourNeighbour);
            bool actualFiveNeighbour = rule.GetNextIterationOfCell(world, fiveNeighbour);

            bool expected = false;
            Assert.Equal(expected, actualTwoNeighbour);
            Assert.Equal(expected, actualFourNeighbour);
            Assert.Equal(expected, actualFiveNeighbour);
        }

        #endregion
    }
}
