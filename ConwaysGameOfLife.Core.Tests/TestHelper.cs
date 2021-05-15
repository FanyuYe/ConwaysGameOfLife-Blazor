using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwaysGameOfLife.Core.Tests
{
    internal class TestHelper
    {
        internal static IWorld CreateMockWorld1D_3(bool[] state)
        {
            var worldMock = new Mock<IWorld>();
            worldMock.Setup(world => world.Dimension).Returns(1);
            worldMock.Setup(world => world.Scale).Returns(3);
            worldMock.Setup(world => world.State).Returns(state);

            return worldMock.Object;
        }

        internal static IWorld CreateMockWorld1D_3()
        {
            return CreateMockWorld1D_3(new bool[3]);
        }

        internal static IWorld CreateMockWorld2D_3x3(bool[] state)
        {
            var worldMock = new Mock<IWorld>();
            worldMock.Setup(world => world.Dimension).Returns(2);
            worldMock.Setup(world => world.Scale).Returns(3);
            worldMock.Setup(world => world.State).Returns(state);

            return worldMock.Object;
        }

        internal static IWorld CreateMockWorld2D_3x3()
        {
            return CreateMockWorld2D_3x3(new bool[9]);
        }
    }
}
