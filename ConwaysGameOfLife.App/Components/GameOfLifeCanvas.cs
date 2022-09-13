using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Blazor.Extensions.Canvas.WebGL;
using ConwaysGameOfLife.Core;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;
using System.Timers;

namespace ConwaysGameOfLife.App.Components
{
    public partial class GameOfLifeCanvas : IDisposable
    {
        private readonly string vsSource = @"
            attribute vec4 aVertexPosition;

            uniform mat4 uModelViewMatrix;
            uniform mat4 uProjectionMatrix;

            void main()
            {
                gl_Position = uProjectionMatrix * uModelViewMatrix * aVertexPosition;
            }
        ";

        private readonly string fsSource = @"
            void main() 
            {
                gl_FragColor = vec4(0.0, 0.8, 0.0, 1.0);
            }
        ";

        private WebGLContext _webGLContext;
        private WebGLShader vtxShader;
        private WebGLShader frgShader;
        private WebGLProgram program;
        private WebGLBuffer buffer;

        private Canvas2DContext _ctx;
        private BECanvasComponent _canvas;
        private ConwaysGameOfLife2D _game;
        private Timer _timer;
        private Stopwatch _stopwatch;

        [Parameter]
        public ConwaysGameOfLife2D GameEngine { get; set; }

        [Parameter]
        public int Width { get; set; } = 400;

        [Parameter]
        public int Height { get; set; } = 400;

        [Parameter]
        public bool IsWebGL { get; set; }

        [Parameter]
        public int Scale { get; set; } = 10;

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
            _timer = new Timer(IsWebGL ? 50 : 250);
            _timer.Elapsed += (s, e) => Tick();

            _stopwatch = new Stopwatch();

            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            GameEngine.Scale = Scale;
            GridSize = 1.0 * Math.Min(Width, Height) / Scale;

            _game = GameEngine;
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

                vtxShader = await _webGLContext.CreateShaderAsync(ShaderType.VERTEX_SHADER);
                frgShader = await _webGLContext.CreateShaderAsync(ShaderType.FRAGMENT_SHADER);
                await _webGLContext.ShaderSourceAsync(vtxShader, vsSource);
                await _webGLContext.ShaderSourceAsync(frgShader, fsSource);
                await _webGLContext.CompileShaderAsync(vtxShader);
                await _webGLContext.CompileShaderAsync(frgShader);

                if (!await _webGLContext.GetShaderParameterAsync<bool>(vtxShader, ShaderParameter.COMPILE_STATUS))
                {
                    await _webGLContext.DeleteShaderAsync(vtxShader);
                    throw new ApplicationException($"An error occurred compiling the shaders: {await _webGLContext.GetErrorAsync()}");
                }
                if (!await _webGLContext.GetShaderParameterAsync<bool>(frgShader, ShaderParameter.COMPILE_STATUS))
                {
                    await _webGLContext.DeleteShaderAsync(frgShader);
                    throw new ApplicationException($"An error occurred compiling the shaders: {await _webGLContext.GetErrorAsync()}");
                }

                program = await _webGLContext.CreateProgramAsync();
                await _webGLContext.AttachShaderAsync(program, vtxShader);
                await _webGLContext.AttachShaderAsync(program, frgShader);
                await _webGLContext.LinkProgramAsync(program);

                if (!await _webGLContext.GetProgramParameterAsync<bool>(program, ProgramParameter.LINK_STATUS))
                {
                    throw new ApplicationException($"Unable to initialize the shader program: {await _webGLContext.GetErrorAsync()}");
                }

                /*
                 * 0 2 - 1 2 - 2 2
                 *     \     \
                 * 0 1 - 1 1 - 2 1
                 *     \     \
                 * 0 0 - 1 0 - 2 0
                 */
                var vertices = new float[Scale * (Scale + 1) * 2 * 2];
                for (int x = 0; x < Scale; ++x)
                {
                    var xoffset = x * (Scale + 1) * 2 * 2;

                    for (int y = 0; y <= Scale; ++y)
                    {
                        var yoffset = y * 2 * 2;

                        // (x, y)
                        vertices[xoffset + yoffset] = x;
                        vertices[xoffset + yoffset + 1] = y;
                        // (x+1, y)
                        vertices[xoffset + yoffset + 2] = x + 1;
                        vertices[xoffset + yoffset + 3] = y;
                    }
                }
                buffer = await _webGLContext.CreateBufferAsync();
                await _webGLContext.BindBufferAsync(BufferType.ARRAY_BUFFER, buffer);
                await _webGLContext.BufferDataAsync<float>(BufferType.ARRAY_BUFFER, vertices, BufferUsageHint.STATIC_DRAW);

                await _webGLContext.VertexAttribPointerAsync(
                    (uint)await _webGLContext.GetAttribLocationAsync(program, "aVertexPosition"),
                    2, DataType.FLOAT, false, 0, 0);
                await _webGLContext.EnableVertexAttribArrayAsync(
                    (uint)await _webGLContext.GetAttribLocationAsync(program, "aVertexPosition"));
                await _webGLContext.UseProgramAsync(program);

                var length = (0.5f * Scale) / (float)Math.Tan(22.5 * Math.PI / 180);
                var m1 = Matrix4x4.CreatePerspectiveFieldOfView(
                    45 * (float)Math.PI / 180,
                    1.0f * Width / Height,
                    0.1f,
                    length);
                var m2 = Matrix4x4.CreateTranslation(new Vector3(-0.5f * Scale, -0.5f * Scale, -1.0f * length));

                await _webGLContext.UniformMatrixAsync(
                    await _webGLContext.GetUniformLocationAsync(program, "uProjectionMatrix"),
                    false,
                    new float[16]
                    {
                    m1.M11, m1.M12, m1.M13, m1.M14,
                    m1.M21, m1.M22, m1.M23, m1.M24,
                    m1.M31, m1.M32, m1.M33, m1.M34,
                    m1.M41, m1.M42, m1.M43, m1.M44
                    });
                await _webGLContext.UniformMatrixAsync(
                    await _webGLContext.GetUniformLocationAsync(program, "uModelViewMatrix"),
                    false,
                    new float[16]
                    {
                    m2.M11, m2.M12, m2.M13, m2.M14,
                    m2.M21, m2.M22, m2.M23, m2.M24,
                    m2.M31, m2.M32, m2.M33, m2.M34,
                    m2.M41, m2.M42, m2.M43, m2.M44
                    });
            }

            await _webGLContext.BeginBatchAsync();

            await _webGLContext.ClearColorAsync(0, 0, 0, 1);
            await _webGLContext.ClearDepthAsync(1);
            await _webGLContext.EnableAsync(EnableCap.DEPTH_TEST);
            await _webGLContext.DepthFuncAsync(CompareFunction.LEQUAL);
            await _webGLContext.ClearAsync(BufferBits.COLOR_BUFFER_BIT | BufferBits.DEPTH_BUFFER_BIT);

            for (int x = 0; x < Scale; ++x)
            {
                var xoffset = x * (Scale + 1) * 2;
                
                for (int y = 0; y < Scale; ++y)
                {
                    var yoffset = y * 2;

                    if (_game.GetState(x, y))
                    {
                        await _webGLContext.DrawArraysAsync(Primitive.TRIANGLE_STRIP, xoffset + yoffset, 4);
                    }
                }
            }

            await _webGLContext.EndBatchAsync();
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

        public void Dispose()
        {
            _timer.Stop();
        }
    }
}
