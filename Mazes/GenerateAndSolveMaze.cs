using MazeGenerator;
using MazeLibrary;
using MazeSolver;
using SDL2;

namespace Mazes
{
    internal class GenerateAndSolveMaze
    {
        private IntPtr _renderer;
        private IntPtr _window;
        private bool _running;

        private IGenerationAlgorithm _generationAlgorithm;
        private ISolverAlgorithm _solverAlgorithm;

        public GenerateAndSolveMaze(IGenerationAlgorithm generationAlgorithm, ISolverAlgorithm solverAlgorithm)
        {
            _generationAlgorithm = generationAlgorithm;
            _solverAlgorithm = solverAlgorithm;
        }

        enum states
        {
            start,
            gen,
            solve,
            done
        }

        public void Go(int videoWidth, int videoHeight)
        {
            initVideo(videoWidth, videoHeight);

            var scalex = videoWidth / _generationAlgorithm.Map.Width;
            var scaley = videoHeight / _generationAlgorithm.Map.Height;
            
            var state = states.start;

            _running = true;

            Thread t;

            while (_running)
            {
                checkEvents();

                clearScreen();

                switch (state)
                {
                    case states.start:
                        t = new Thread(() => _generationAlgorithm.Generate(-1));
                        t.Start();
                        Thread.Sleep(1000);
                        state = states.gen;
                        break;

                    case states.gen:
                        if (_generationAlgorithm.IsRunning == false)
                        {
                            t = new Thread(() => _solverAlgorithm.Solve(_generationAlgorithm.Map, 50));
                            t.Start();
                            Thread.Sleep(1000);
                            state = states.solve;
                        }
                        else
                        {
                            renderGenerator(scalex, scaley);
                        }
                        break;

                    case states.solve:
                        if (_solverAlgorithm.IsRunning == false)
                        {
                            state = states.done;
                        }
                        else
                        {
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

            _generationAlgorithm.Stop();
            _solverAlgorithm.Stop();

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
                    if (x == _generationAlgorithm.CurrentX && y == _generationAlgorithm.CurrentY)
                    {
                        drawSelectRect(x, y, scalex, scaley);
                    }
                    if (_generationAlgorithm.Visited(x, y))
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
                    if (x == _solverAlgorithm.CurrentX && y == _solverAlgorithm.CurrentY)
                    {
                        drawSelectRect(x, y, scalex, scaley);
                    }
                    drawWalls(x, y, scalex, scaley, c);
                }
            }
            var max = _solverAlgorithm.Locations.Count();
            for (int i = 0; i < max && i < 255; i++)
            {
                var border = 3;
                SDL.SDL_SetRenderDrawColor(_renderer, (byte)((255/max) * i), 0, 0, 255);
                var rect = new SDL.SDL_Rect
                {
                    x = (_solverAlgorithm.Locations[i].X * scalex) + border,
                    y = (_solverAlgorithm.Locations[i].Y * scaley) + border,
                    w = scalex - (border  *2) - 1,
                    h = scaley - (border * 2) - 1
                };
                SDL.SDL_RenderFillRect(_renderer, ref rect);
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
