using Moq;
using System;
using Xunit;

namespace ConwaysGameOfLife.Core.Tests
{
    public class ConwaysGameOfLife2DTest
    {
        #region ConwaysGameOfLife2D(IWorldViewer, IWorldEdior, ISimulator, ICoordinateConverter)

        [Fact]
        public void ConwaysGameOfLife2D_ParameterWorldViewerIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ConwaysGameOfLife2D(null, 
                Mock.Of<IWorldEditor>(), Mock.Of<ISimulator>(), Mock.Of<ICoordinateConverter>()));
        }

        [Fact]
        public void ConwaysGameOfLife2D_ParameterWorldEditorIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ConwaysGameOfLife2D(Mock.Of<IWorldViewer>(),
                null, Mock.Of<ISimulator>(), Mock.Of<ICoordinateConverter>()));
        }

        [Fact]
        public void ConwaysGameOfLife2D_ParameterSimulatorIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ConwaysGameOfLife2D(Mock.Of<IWorldViewer>(),
                Mock.Of<IWorldEditor>(), null, Mock.Of<ICoordinateConverter>()));
        }

        [Fact]
        public void ConwaysGameOfLife2D_ParameterCoordinateConverterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ConwaysGameOfLife2D(Mock.Of<IWorldViewer>(),
                Mock.Of<IWorldEditor>(), Mock.Of<ISimulator>(), null));
        }

        #endregion

        #region CreateClassicGame(int)
        
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void CreateClassicGame_ReturnsGameWithScaleSameAsProvided(int scale)
        {
            var game = ConwaysGameOfLife2D.CreateClassicGame(scale);

            Assert.Equal(scale, game.Scale);
        }

        #endregion

        #region Run()

        [Fact]
        public void Run_Invoke_SimulatorTickExactlyOnce()
        {
            var simMock = new Mock<ISimulator>();
            var game = new ConwaysGameOfLife2D(Mock.Of<IWorldViewer>(), Mock.Of<IWorldEditor>(), 
                simMock.Object, Mock.Of<ICoordinateConverter>());
            simMock.Setup(sim => sim.Tick());

            game.Run();

            simMock.Verify(sim => sim.Tick(), Times.Once);
        }

        #endregion
    }
}
