using Moq;
using System;
using System.Linq;
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
            var oneNeighbourWorldInterpreterMock = CreateWorldInterpreterMock(9, 1);
            var zeroNeighbourWorldInterpreterMock = CreateWorldInterpreterMock(9, 0);
            var oneNeighbourRule = new Rule(CreateMockRuleConfiguration(), oneNeighbourWorldInterpreterMock);
            var zeroNeighbourRule = new Rule(CreateMockRuleConfiguration(), zeroNeighbourWorldInterpreterMock);

            int aliveOneNeighbour = 0;
            int aliveZeroNeighbour = 6;
            int emptyOneNeighbour = 2;
            int emptyZeroNeighbour = 8;
            bool actualAliveOneNeighbour = oneNeighbourRule.GetNextIterationOfCell(world, aliveOneNeighbour);
            bool actualAliveZeroNeighbour = zeroNeighbourRule.GetNextIterationOfCell(world, aliveZeroNeighbour);
            bool actualEmptyOneNeighbour = oneNeighbourRule.GetNextIterationOfCell(world, emptyOneNeighbour);
            bool actualEmptyZeroNeighbour = zeroNeighbourRule.GetNextIterationOfCell(world, emptyZeroNeighbour);

            bool expected = false;
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
            var fourNeighbourWorldInterpreterMock = CreateWorldInterpreterMock(9, 4);
            var fiveNeighbourWorldInterpreterMock = CreateWorldInterpreterMock(9, 5);
            var fourNeighbourRule = new Rule(CreateMockRuleConfiguration(), fourNeighbourWorldInterpreterMock);
            var fiveNeighbourRule = new Rule(CreateMockRuleConfiguration(), fiveNeighbourWorldInterpreterMock);


            int aliveFourNeighbour = 1;
            int emptyFiveNeighbour = 7;
            bool actualAliveFourNeighbour = fourNeighbourRule.GetNextIterationOfCell(world, aliveFourNeighbour);
            bool actualEmptyFiveNeighbour = fiveNeighbourRule.GetNextIterationOfCell(world, emptyFiveNeighbour);

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
            var twoNeighbourWorldInterpreterMock = CreateWorldInterpreterMock(9, 2);
            var threeNeighbourWorldInterpreterMock = CreateWorldInterpreterMock(9, 3);
            var twoNeighbourRule = new Rule(CreateMockRuleConfiguration(), twoNeighbourWorldInterpreterMock);
            var threeNeighbourRule = new Rule(CreateMockRuleConfiguration(), threeNeighbourWorldInterpreterMock);

            int twoNeighbour = 0;
            int threeNeighbour = 8;
            bool actualTwoNeighbour = twoNeighbourRule.GetNextIterationOfCell(world, twoNeighbour);
            bool actualThreeNeighbour = threeNeighbourRule.GetNextIterationOfCell(world, threeNeighbour);

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
            var threeNeighbourWorldInterpreterMock = CreateWorldInterpreterMock(9, 3);
            var threeNeighbourRule = new Rule(CreateMockRuleConfiguration(), threeNeighbourWorldInterpreterMock);

            int threeNeighbour = 0;
            bool actual = threeNeighbourRule.GetNextIterationOfCell(world, threeNeighbour);

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
            var twoNeighbourWorldInterpreterMock = CreateWorldInterpreterMock(9, 2);
            var fourNeighbourWorldInterpreterMock = CreateWorldInterpreterMock(9, 4);
            var fiveNeighbourWorldInterpreterMock = CreateWorldInterpreterMock(9, 5);
            var twoNeighbourRule = new Rule(CreateMockRuleConfiguration(), twoNeighbourWorldInterpreterMock);
            var fourNeighbourRule = new Rule(CreateMockRuleConfiguration(), fourNeighbourWorldInterpreterMock);
            var fiveNeighbourRule = new Rule(CreateMockRuleConfiguration(), fiveNeighbourWorldInterpreterMock);

            int twoNeighbour = 0;
            int fourNeighbour = 1;
            int fiveNeighbour = 7;
            bool actualTwoNeighbour = twoNeighbourRule.GetNextIterationOfCell(world, twoNeighbour);
            bool actualFourNeighbour = fourNeighbourRule.GetNextIterationOfCell(world, fourNeighbour);
            bool actualFiveNeighbour = fiveNeighbourRule.GetNextIterationOfCell(world, fiveNeighbour);

            bool expected = false;
            Assert.Equal(expected, actualTwoNeighbour);
            Assert.Equal(expected, actualFourNeighbour);
            Assert.Equal(expected, actualFiveNeighbour);
        }

        #endregion
    }
}
