using Moq;
using System;
using Xunit;

namespace ConwaysGameOfLife.Core.Tests
{
    public class RuleTest
    {
        #region Rule(IRuleConfigurable)

        [Fact]
        public void Constructor_ConfigParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Rule(null));
        }

        #endregion

        #region ApplyRuleToCell(IWorld, int)

        [Fact]
        public void ApplyRuleToCell_AliveOrEmptyCell_NeighbourLessThanUnderpopulationThreshold_ReturnFalse()
        {
            const int underpopulationThreshold = 2;
            const int overpopulationThreshold = 3;
            const int reproductionNeighbourCount = 3;
            var ruleConfigMock = new Mock<IRuleConfigurable>();
            ruleConfigMock.SetupProperty(cfg => cfg.UnderpopulationThreshold, underpopulationThreshold);
            ruleConfigMock.SetupProperty(cfg => cfg.OverpopulationThreshold, overpopulationThreshold);
            ruleConfigMock.SetupProperty(cfg => cfg.ReproductionNeighbourCount, reproductionNeighbourCount);

            const int dimension = 2;
            const int scale = 3;
            const int size = scale * scale;
            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, dimension);
            worldMock.SetupProperty(world => world.Scale, scale);
            worldMock.SetupProperty(world => world.State, new bool[size]
                {
                    true,  true,  false,
                    false, false, false,
                    true,  false, false
                });

            var world = worldMock.Object;
            var config = ruleConfigMock.Object;
            var rule = new Rule(config);

            int aliveOneNeighbour = 0;
            int aliveZeroNeighbour = 6;
            int emptyOneNeighbour = 2;
            int emptyZeroNeighbour = 8;

            bool expected = false;
            bool actualAliveOneNeighbour = rule.ApplyRuleToCell(world, aliveOneNeighbour);
            bool actualAliveZeroNeighbour = rule.ApplyRuleToCell(world, aliveZeroNeighbour);
            bool actualEmptyOneNeighbour = rule.ApplyRuleToCell(world, emptyOneNeighbour);
            bool actualEmptyZeroNeighbour = rule.ApplyRuleToCell(world, emptyZeroNeighbour);

            Assert.Equal(expected, actualAliveOneNeighbour);
            Assert.Equal(expected, actualAliveZeroNeighbour);
            Assert.Equal(expected, actualEmptyOneNeighbour);
            Assert.Equal(expected, actualEmptyZeroNeighbour);
        }

        [Fact]
        public void ApplyRuleToCell_AliveOrEmptyCell_NeighbourGreaterThanOverpopulationThreshold_ReturnFalse()
        {
            const int underpopulationThreshold = 2;
            const int overpopulationThreshold = 3;
            const int reproductionNeighbourCount = 3;
            var ruleConfigMock = new Mock<IRuleConfigurable>();
            ruleConfigMock.SetupProperty(cfg => cfg.UnderpopulationThreshold, underpopulationThreshold);
            ruleConfigMock.SetupProperty(cfg => cfg.OverpopulationThreshold, overpopulationThreshold);
            ruleConfigMock.SetupProperty(cfg => cfg.ReproductionNeighbourCount, reproductionNeighbourCount);

            const int dimension = 2;
            const int scale = 3;
            const int size = scale * scale;
            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, dimension);
            worldMock.SetupProperty(world => world.Scale, scale);
            worldMock.SetupProperty(world => world.State, new bool[size]
                {
                    true, true,  false,
                    true, true,  true,
                    true, false, true
                });

            var world = worldMock.Object;
            var config = ruleConfigMock.Object;
            var rule = new Rule(config);

            int aliveFourNeighbour = 1;
            int emptyFiveNeighbour = 7;

            bool expected = false;
            bool actualAliveFourNeighbour = rule.ApplyRuleToCell(world, aliveFourNeighbour);
            bool actualEmptyFiveNeighbour = rule.ApplyRuleToCell(world, emptyFiveNeighbour);

            Assert.Equal(expected, actualAliveFourNeighbour);
            Assert.Equal(expected, actualEmptyFiveNeighbour);
        }

        [Fact]
        public void ApplyRuleToCell_AliveCell_NeighbourBetweenUnderpopulationAndOverpopulationThreshold_ReturnTrue()
        {
            const int underpopulationThreshold = 2;
            const int overpopulationThreshold = 3;
            const int reproductionNeighbourCount = 3;
            var ruleConfigMock = new Mock<IRuleConfigurable>();
            ruleConfigMock.SetupProperty(cfg => cfg.UnderpopulationThreshold, underpopulationThreshold);
            ruleConfigMock.SetupProperty(cfg => cfg.OverpopulationThreshold, overpopulationThreshold);
            ruleConfigMock.SetupProperty(cfg => cfg.ReproductionNeighbourCount, reproductionNeighbourCount);

            const int dimension = 2;
            const int scale = 3;
            const int size = scale * scale;
            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, dimension);
            worldMock.SetupProperty(world => world.Scale, scale);
            worldMock.SetupProperty(world => world.State, new bool[size]
                {
                    true,  false, false,
                    true,  true,  true,
                    false, true,  true
                });

            var world = worldMock.Object;
            var config = ruleConfigMock.Object;
            var rule = new Rule(config);

            int twoNeighbour = 0;
            int threeNeighbour = 8;

            bool expected = true;
            bool actualTwoNeighbour = rule.ApplyRuleToCell(world, twoNeighbour);
            bool actualThreeNeighbour = rule.ApplyRuleToCell(world, threeNeighbour);

            Assert.Equal(expected, actualTwoNeighbour);
            Assert.Equal(expected, actualThreeNeighbour);
        }

        [Fact]
        public void ApplyRuleToCell_EmptyCell_NeighbourEqualToReproductionThreshold_ReturnTrue()
        {
            const int underpopulationThreshold = 2;
            const int overpopulationThreshold = 3;
            const int reproductionNeighbourCount = 3;
            var ruleConfigMock = new Mock<IRuleConfigurable>();
            ruleConfigMock.SetupProperty(cfg => cfg.UnderpopulationThreshold, underpopulationThreshold);
            ruleConfigMock.SetupProperty(cfg => cfg.OverpopulationThreshold, overpopulationThreshold);
            ruleConfigMock.SetupProperty(cfg => cfg.ReproductionNeighbourCount, reproductionNeighbourCount);

            const int dimension = 2;
            const int scale = 3;
            const int size = scale * scale;
            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, dimension);
            worldMock.SetupProperty(world => world.Scale, scale);
            worldMock.SetupProperty(world => world.State, new bool[size]
                {
                    false, true,  false,
                    true,  true,  false,
                    false, false, false
                });

            var world = worldMock.Object;
            var config = ruleConfigMock.Object;
            var rule = new Rule(config);

            int threeNeighbour = 0;

            bool expected = true;
            bool actual = rule.ApplyRuleToCell(world, threeNeighbour);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ApplyRuleToCell_EmptyCell_NeighbourNotEqualToReproductionThreshold_ReturnFalse()
        {
            const int underpopulationThreshold = 2;
            const int overpopulationThreshold = 3;
            const int reproductionNeighbourCount = 3;
            var ruleConfigMock = new Mock<IRuleConfigurable>();
            ruleConfigMock.SetupProperty(cfg => cfg.UnderpopulationThreshold, underpopulationThreshold);
            ruleConfigMock.SetupProperty(cfg => cfg.OverpopulationThreshold, overpopulationThreshold);
            ruleConfigMock.SetupProperty(cfg => cfg.ReproductionNeighbourCount, reproductionNeighbourCount);

            const int dimension = 2;
            const int scale = 3;
            const int size = scale * scale;
            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, dimension);
            worldMock.SetupProperty(world => world.Scale, scale);
            worldMock.SetupProperty(world => world.State, new bool[size]
                {
                    false, false, true,
                    true,  true,  true,
                    true,  false, true
                });

            var world = worldMock.Object;
            var config = ruleConfigMock.Object;
            var rule = new Rule(config);

            int twoNeighbour = 0;
            int fourNeighbour = 1;
            int fiveNeighbour = 7;

            bool expected = false;
            bool actualTwoNeighbour = rule.ApplyRuleToCell(world, twoNeighbour);
            bool actualFourNeighbour = rule.ApplyRuleToCell(world, fourNeighbour);
            bool actualFiveNeighbour = rule.ApplyRuleToCell(world, fiveNeighbour);

            Assert.Equal(expected, actualTwoNeighbour);
            Assert.Equal(expected, actualFourNeighbour);
            Assert.Equal(expected, actualFiveNeighbour);
        }

        #endregion
    }
}
