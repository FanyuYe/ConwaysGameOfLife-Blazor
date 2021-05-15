using Moq;

namespace ConwaysGameOfLife.Core.Tests
{
    internal class TestHelper
    {
        internal static IWorld CreateMockWorld1D_3(bool[] state)
        {
            var worldMock = new Mock<IWorld>();
            worldMock.SetupGet(world => world.Dimension).Returns(1);
            worldMock.SetupGet(world => world.Scale).Returns(3);
            worldMock.SetupGet(world => world.State).Returns(state);

            return worldMock.Object;
        }

        internal static IWorld CreateMockWorld1D_3()
        {
            return CreateMockWorld1D_3(new bool[3]);
        }

        internal static IWorld CreateMockWorld2D_3x3(bool[] state)
        {
            var worldMock = new Mock<IWorld>();
            worldMock.SetupGet(world => world.Dimension).Returns(2);
            worldMock.SetupGet(world => world.Scale).Returns(3);
            worldMock.SetupGet(world => world.State).Returns(state);

            return worldMock.Object;
        }

        internal static IWorld CreateMockWorld2D_3x3()
        {
            return CreateMockWorld2D_3x3(new bool[9]);
        }
    }
}
