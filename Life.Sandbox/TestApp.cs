using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Life.Core.Basic;
using Life.Core.Caliburn.Micro;

namespace Life.Sandbox
{
    public class TestApp : 
        IHandle<GameTimeUpdated>, 
        IHandle<GamePaused>, 
        IHandle<PropertyChanged>
    {
        private MainLoop _mainLoop;
        private volatile object _consoleLock = new object();

        public TestApp()
        {
            this.Subscribe();
        }

        public void Run()
        {
            var environment = new GameEnvironment();
            
            var entities = new List<IGameEntity>();
            entities.Add(new Human(environment, new DateTime(1980, 1, 12)));

            var engine = new GameEngine(environment);
            engine.Initialize(entities);

            _mainLoop = new MainLoop(engine);
            _mainLoop.Start();

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                HandleInput(key);
            } while (key.Key != ConsoleKey.Escape);

            _mainLoop.Stop();
        }

        private void HandleInput(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.RightArrow:
                    _mainLoop.IncreaseSpeed();
                    break;
                case ConsoleKey.LeftArrow:
                    _mainLoop.DecreaseSpeed();
                    break;
                case ConsoleKey.Spacebar:
                    _mainLoop.TogglePause();
                    break;
            }
        }

        public void Handle(GameTimeUpdated message)
        {
            WriteLine(0, 0, "Current game time: {0}", message.GameTime);
        }

        public void Handle(GamePaused message)
        {
            WriteLine(0, 1, "Game paused: {0}", message.IsPaused);
        }


        public void Handle(PropertyChanged message)
        {
            WriteLine(0, _currentCursor++, "{0} - {1}: {2}", message.Sender, message.PropertyName, message.Value);
        }

        private int _currentCursor = 2;


        private void WriteLine(int x, int y, string format, params object[] args)
        {
            lock (_consoleLock)
            {
                Console.SetCursorPosition(0, y);
                Console.Write(" ".Repeat(Console.WindowWidth));
                Console.SetCursorPosition(x, y);
                Console.WriteLine(format, args);
            }
        }
    }
}