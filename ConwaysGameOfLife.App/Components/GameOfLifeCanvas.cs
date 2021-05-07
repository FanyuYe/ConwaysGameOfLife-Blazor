using Blazor.Extensions;
using Blazor.Extensions.Canvas.WebGL;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace ConwaysGameOfLife.App.Components
{
    public partial class GameOfLifeCanvas
    {
        private WebGLContext ctx;
        private BECanvasComponent canvas;

        [Parameter]
        public int Width { get; set; }

        [Parameter]
        public int Height { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            this.ctx = await this.canvas.CreateWebGLAsync();

            await this.ctx.ClearColorAsync(0, 0, 0, 1);
            await this.ctx.ClearAsync(BufferBits.COLOR_BUFFER_BIT);
        }
    }
}
