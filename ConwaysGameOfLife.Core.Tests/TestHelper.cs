using Moq;

namespace ConwaysGameOfLife.Core.Tests
{
    internal class TestHelper
    {
        internal static IWorld CreateMockWorld1D_3(bool[] state)
        {
            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, 1);
            worldMock.SetupProperty(world => world.Scale, 3);
            worldMock.SetupProperty(world => world.State, state);

            return worldMock.Object;
        }

        internal static IWorld CreateMockWorld1D_3()
        {
            return CreateMockWorld1D_3(new bool[3]);
        }

        internal static IWorld CreateMockWorld2D_3x3(bool[] state)
        {
            var worldMock = new Mock<IWorld>();
            worldMock.SetupProperty(world => world.Dimension, 2);
            worldMock.SetupProperty(world => world.Scale, 3);
            worldMock.SetupProperty(world => world.State, state);

            return worldMock.Object;
        }

        internal static IWorld CreateMockWorld2D_3x3()
        {
            return CreateMockWorld2D_3x3(new bool[9]);
        }

        internal static ICoordinateConverter CreateMockCoordinateConverter(int maximumScaleToSetup = 5)
        {
            var converterMock = new Mock<ICoordinateConverter>(MockBehavior.Strict);
            const int ONE = 1;
            const int TWO = 2;
            const int THREE = 3;
            // 1D
            for (int scale = 1; scale <= maximumScaleToSetup; ++scale)
            {
                for (int x = 0; x < scale; ++x)
                {
                    converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(scale, new int[ONE] { x })).Returns(x);
                    converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(ONE, scale, x)).Returns(new int[ONE] { x });
                }
            }
            // 2D
            for (int scale = 1; scale <= maximumScaleToSetup; ++scale)
            {
                for (int y = 0; y < scale; ++y)
                {
                    for (int x = 0; x < scale; ++x)
                    {
                        int coo = x + scale * y;
                        converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(scale, new int[TWO] { x, y })).Returns(coo);
                        converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(TWO, scale, coo)).Returns(new int[TWO] { x, y });
                    }
                }
            }
            // 3D
            for (int scale = 1; scale <= maximumScaleToSetup; ++scale)
            {
                for (int z = 0; z < scale; ++z)
                {
                    for (int y = 0; y < scale; ++y)
                    {
                        for (int x = 0; x < scale; ++x)
                        {
                            int coo = x + scale * y + scale * scale * z;
                            converterMock.Setup(mock => mock.ConvertCoordinateMultiToSingle(scale, new int[THREE] { x, y, z })).Returns(coo);
                            converterMock.Setup(mock => mock.ConvertCoordinateSingleToMulti(THREE, scale, coo)).Returns(new int[THREE] { x, y, z });
                        }
                    }
                }
            }

            return converterMock.Object;
        }
    }
}
