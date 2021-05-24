using System;
using Xunit;

namespace ConwaysGameOfLife.Core.Tests
{
    public class WorldViewerTest
    {
        #region Constructor

        [Fact]
        public void WorldViewer_ParameterWorldIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WorldViewer(null));
        }

        #endregion
    }
}
