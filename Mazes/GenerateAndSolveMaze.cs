using MazeLibrary;
using SDL2;

namespace Mazes
{
    internal class GenerateAndSolveMaze
    {
        private IntPtr _renderer;
        private IntPtr _window;
        private bool _running;
        private int _width;
        private int _height;

        private IGenerationAlgorithm _generationAlgorithm;

        private ISolverAlgorithm _solverAlgorithm;
        private ISolverAlgorithmFactory _solverAlgorithmFactory;

        private List<Coordinates> _solverHistory;

        public GenerateAndSolveMaze(IGenerationAlgorithm generationAlgorithm, ISolverAlgorithmFactory solverAlgorithmFactory, MazeConfiguration configuration)
        {
            _generationAlgorithm = generationAlgorithm;
            _solverAlgorithmFactory = solverAlgorithmFactory;
            _solverHistory = new List<Coordinates>();
            _width = configuration.VideoWidth;
            _height = configuration.VideoHeight;
        }

        enum states
        {
            start,
            gen,
            solve,
            done
        }

        public void Go()
        {
            initVideo(_width, _height);

            var scalex = _width / _generationAlgorithm.Map.Width;
            var scaley = _height / _generationAlgorithm.Map.Height;
            
            var state = states.start;

            _running = true;

            while (_running)
            {
                checkEvents();

                clearScreen();

                switch (state)
                {
                    case states.start:
                        Thread.Sleep(1000);
                        state = states.gen;
                        break;

                    case states.gen:
                        if (_generationAlgorithm.Complete == true)
                        {
                            Thread.Sleep(1000);
                            state = states.solve;
                            _solverAlgorithm = _solverAlgorithmFactory.Create(_generationAlgorithm.Map);
                            _solverHistory.Add(_solverAlgorithm.CurrentCoordinates);
                        }
                        else
                        {
                            // TODO: calculate steps to take
                            for (int i = 0; i < 100; i++)
                            {
                                _generationAlgorithm.Step();
                            }
                            renderGenerator(scalex, scaley);
                        }
                        break;

                    case states.solve:
                        if (_solverAlgorithm.Complete == true)
                        {
                            Thread.Sleep(1000);
                            state = states.done;
                        }
                        else
                        {
                            // TODO: calculate steps to take
                            for (int i = 0; i < 1; i++)
                            {
                                _solverAlgorithm.Step();
                                _solverHistory.Add(_solverAlgorithm.CurrentCoordinates);
                            }
                            renderSolver(scalex, scaley);
                        }
                        break;

                    case states.done:
                        renderSolver(scalex, scaley);
                        break;
                }

                SDL.SDL_RenderPresent(_renderer);
            }

            SDL.SDL_DestroyRenderer(_renderer);
            SDL.SDL_DestroyWindow(_window);
            SDL.SDL_Quit();
        }

        private void clearScreen()
        {
            if (SDL.SDL_SetRenderDrawColor(_renderer, 0, 0, 0, 255) < 0)
            {
                Console.WriteLine($"Colour error: {SDL.SDL_GetError()}");
            }

            if (SDL.SDL_RenderClear(_renderer) < 0)
            {
                Console.WriteLine($"Clear error: {SDL.SDL_GetError()}");
            }
        }

        private void checkEvents()
        {
            while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        _running = false;
                        break;
                }
            }
        }

        private void initVideo(int videoWidth, int videoHeight)
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine($"Init error: {SDL.SDL_GetError()}");
            }

            _window = SDL.SDL_CreateWindow("Mazes", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, videoWidth, videoHeight, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

            if (_window == IntPtr.Zero)
            {
                Console.WriteLine($"Window error: {SDL.SDL_GetError()}");
            }

            _renderer = SDL.SDL_CreateRenderer(_window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            if (_renderer == IntPtr.Zero)
            {
                Console.WriteLine($"Renderer error: {SDL.SDL_GetError()}");
            }
        }

        private void renderGenerator(int scalex, int scaley)
        {
            for (int x = 0; x < _generationAlgorithm.Map.Width; x++)
            {
                for (int y = 0; y < _generationAlgorithm.Map.Height; y++)
                {
                    var c = _generationAlgorithm.Map[x, y];
                    if (x == _generationAlgorithm.CurrentCoordinates.X && y == _generationAlgorithm.CurrentCoordinates.Y)
                    {
                        drawSelectRect(x, y, scalex, scaley);
                    }
                    if (c.Walls.Any(w => w.Value == false))
                    {
                        drawWalls(x, y, scalex, scaley, c);
                    }
                    else
                    {
                        drawInnerRect(x, y, scalex, scaley);
                    }
                }
            }
        }

        private void renderSolver(int scalex, int scaley)
        {
            for (int x = 0; x < _solverAlgorithm.Map.Width; x++)
            {
                for (int y = 0; y < _solverAlgorithm.Map.Height; y++)
                {
                    var c = _solverAlgorithm.Map[x, y];
                    if (x == _solverAlgorithm.CurrentCoordinates.X && y == _solverAlgorithm.CurrentCoordinates.Y)
                    {
                        drawSelectRect(x, y, scalex, scaley);
                    }
                    drawWalls(x, y, scalex, scaley, c);
                }
            }

            // probably won't work for different algorithms
            var max = _solverHistory.Count();
            for (int i = 0; i < max; i++)
            {
                byte r = (byte)((255.0 / max) * i);
                byte g = 0x00;
                byte b = 0x00;
                fillInnerRect(_solverHistory[i].X, _solverHistory[i].Y, scalex, scaley, r, g, b);
            }
        }

        private void drawSelectRect(int x, int y, int scalex, int scaley)
        {
            var border = 3;
            SDL.SDL_SetRenderDrawColor(_renderer, 255, 255, 0, 255);
            var rect = new SDL.SDL_Rect
            {
                x = (x * scalex) - border,
                y = (y * scaley) - border,
                w = scalex + (border * 2) - 1,
                h = scaley + (border * 2) - 1
            };
            SDL.SDL_RenderDrawRect(_renderer, ref rect);
        }

        private void drawInnerRect(int x, int y, int scalex, int scaley)
        {
            SDL.SDL_SetRenderDrawColor(_renderer, 255, 255, 255, 255);
            var rect = new SDL.SDL_Rect
            {
                x = x * scalex,
                y = y * scaley,
                w = scalex - 1,
                h = scaley - 1
            };
            SDL.SDL_RenderDrawRect(_renderer, ref rect);
        }

        private void fillInnerRect(int x, int y, int scalex, int scaley, byte r, byte g, byte b)
        {
            var border = 3;
            SDL.SDL_SetRenderDrawColor(_renderer, r, g, b, 255);
            var rect = new SDL.SDL_Rect
            {
                x = (x * scalex) + border,
                y = (y * scaley) + border,
                w = scalex - (border * 2) - 1,
                h = scaley - (border * 2) - 1
            };
            SDL.SDL_RenderFillRect(_renderer, ref rect);
        }

        private void drawWalls(int x, int y, int scalex, int scaley, Cell c)
        {
            var left = x * scalex;
            var top = y * scaley;
            var right = (x * scalex) + scalex - 1;
            var bot = (y * scaley) + scaley - 1;
            SDL.SDL_SetRenderDrawColor(_renderer, 255, 0, 0, 255);
            if (c.Walls[CompassPoint.North])
            {
                SDL.SDL_RenderDrawLine(_renderer, left, top, right, top);
            }
            if (c.Walls[CompassPoint.East])
            {
                SDL.SDL_RenderDrawLine(_renderer, right, top, right, bot);
            }
            if (c.Walls[CompassPoint.South])
            {
                SDL.SDL_RenderDrawLine(_renderer, right, bot, left, bot);
            }
            if (c.Walls[CompassPoint.West])
            {
                SDL.SDL_RenderDrawLine(_renderer, left, bot, left, top);
            }
        }
    }
}
