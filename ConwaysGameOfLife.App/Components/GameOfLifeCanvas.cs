using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Blazor.Extensions.Canvas.WebGL;
using ConwaysGameOfLife.Core;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

namespace ConwaysGameOfLife.App.Components
{
    public partial class GameOfLifeCanvas
    {
        private Canvas2DContext _ctx;
        private WebGLContext _webGLContext;
        private BECanvasComponent _canvas;
        private ConwaysGameOfLife2D _game;
        private Timer _timer;
        private Stopwatch _stopwatch;

        [Parameter]
        public int Width { get; set; } = 400;

        [Parameter]
        public int Height { get; set; } = 400;

        [Parameter]
        public int Scale { get; set; } = 10;

        [Parameter]
        public bool IsWebGL { get; set; }

        public double GridSize { get; private set; }

        public int Iteration { get; private set; }

        public int LiveCellCount { get; private set; }

        public long TickCostInMilliSecond { get; private set; }

        public long RenderCostInMilliSecond { get; private set; }

        private void Run()
        {
            _timer.Start();
        }

        private void Pause()
        {
            _timer.Stop();
        }

        private void Tick()
        {
            _stopwatch.Start();
            _game.Run();
            _stopwatch.Stop();
            TickCostInMilliSecond = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Reset();

            Iteration++;
            UpdateLiveCellCount();
            StateHasChanged();
        }

        private void Reset()
        {
            _game.Reset();
            Iteration = 0;
            UpdateLiveCellCount();
            StateHasChanged();
        }

        private void Random()
        {
            SetRandomSeed(0.5);
            _game.Save();
            Iteration = 0;
            UpdateLiveCellCount();
            StateHasChanged();
        }

        private void UpdateLiveCellCount()
        {
            int i = 0;

            for (int x = 0; x < Scale; ++x)
            {
                for (int y = 0; y < Scale; ++y)
                {
                    if (_game.GetState(x, y))
                        i++;
                }
            }

            LiveCellCount = i;
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

        protected override async Task OnInitializedAsync()
        {
            _timer = new Timer(250);
            _timer.Elapsed += (s, e) => Tick();

            _stopwatch = new Stopwatch();

            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            GridSize = 1.0 * Math.Min(Width, Height) / Scale;

            _game = ConwaysGameOfLife2D.CreateClassicGame(Scale);
            Random();
            _game.Save();
            StateHasChanged();

            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            _stopwatch.Start();

            if (IsWebGL)
            {
                await OnAfterRenderAsyncWebGL(firstRender);
            }
            else
            {
                await OnAfterRenderAsyncCanvas2D(firstRender);
            }

            _stopwatch.Stop();
            RenderCostInMilliSecond = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Reset();
        }

        private async Task OnAfterRenderAsyncWebGL(bool firstRender)
        {
            if (firstRender)
            {
                _webGLContext = await _canvas.CreateWebGLAsync();
            }

            await this._webGLContext.ClearColorAsync(0, 0, 0, 1);
            await this._webGLContext.ClearAsync(BufferBits.COLOR_BUFFER_BIT);
        }

        private async Task OnAfterRenderAsyncCanvas2D(bool firstRender)
        {
            if (firstRender)
            {
                _ctx = await _canvas.CreateCanvas2DAsync();
                await _ctx.SetFillStyleAsync("green");
                await _ctx.SetStrokeStyleAsync("#777777");
                await _ctx.SetLineWidthAsync(1);
            }

            await _ctx.ClearRectAsync(0, 0, GridSize * Scale, GridSize * Scale);

            await _ctx.BeginPathAsync();

            for (int i = 0; i <= Scale; ++i)
            {
                await _ctx.MoveToAsync(GridSize * i, 0);
                await _ctx.LineToAsync(GridSize * i, GridSize * Scale);
                await _ctx.MoveToAsync(0, GridSize * i);
                await _ctx.LineToAsync(GridSize * Scale, GridSize * i);
            }

            for (int y = 0; y < Scale; ++y)
            {
                for (int x = 0; x < Scale; ++x)
                {
                    if (_game.GetState(x, y))
                    {
                        await _ctx.RectAsync(x * GridSize + 1, y * GridSize + 1, GridSize - 2, GridSize - 2);
                    }
                }
            }

            await _ctx.StrokeAsync();
            await _ctx.FillAsync();
        }
    }
}
