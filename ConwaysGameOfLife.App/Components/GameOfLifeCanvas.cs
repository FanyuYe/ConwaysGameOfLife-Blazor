using Blazor.Extensions;
using Blazor.Extensions.Canvas.WebGL;
using ConwaysGameOfLife.Core;
using Microsoft.AspNetCore.Components;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ConwaysGameOfLife.App.Components
{
    public partial class GameOfLifeCanvas
    {
        private WebGLContext _ctx;
        private BECanvasComponent _canvas;
        private ConwaysGameOfLife2D _game;

        [Parameter]
        public int Width { get; set; } = 400;

        [Parameter]
        public int Height { get; set; } = 400;

        [Parameter]
        public int Scale { get; set; } = 10;

        public int Iteration { get; private set; }

        public string GameAsASCII { get; private set; }

        private void Draw()
        {
            GameAsASCII = PrintGameASCII();
        }

        private void Tick()
        {
            _game.Run();
            Iteration++;
            Draw();
        }

        private void Reset()
        {
            _game.Reset();
            Iteration = 0;
            Draw();
        }

        private void Random()
        {
            SetRandomSeed(0.5);
            _game.Save();
            Iteration = 0;
            Draw();
        }

        private string PrintGameASCII()
        {
            var sb = new StringBuilder();

            for (int y = 0; y < _game.Scale; ++y)
            {
                for (int x = 0; x < _game.Scale; ++x)
                {
                    sb.Append(_game.GetState(x, y) ? 'X' : 'O');
                }

                sb.AppendLine();
            }

            return sb.ToString().TrimEnd();
        }

        private void SetRandomSeed(double percentage)
        {
            Random rand = new Random(DateTime.UtcNow.Second);

            for (int y = 0; y < _game.Scale; ++y)
            {
                for (int x = 0; x < _game.Scale; ++x)
                {
                    _game.SetState(x, y, rand.NextDouble() <= percentage);
                }
            }
        }

        protected override void OnParametersSet()
        {
            _game = ConwaysGameOfLife2D.CreateClassicGame(Scale);
            Random();
            _game.Save();
            Draw();

            base.OnParametersSet();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            _ctx = await _canvas.CreateWebGLAsync();

            await _ctx.ClearColorAsync(0, 0, 0, 1);
            await _ctx.ClearAsync(BufferBits.COLOR_BUFFER_BIT);
        }
    }
}
