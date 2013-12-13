using System;
using Life.Core.Caliburn.Micro;

namespace Life.Core.Basic
{
    public class GameEngine
    {
        private DateTime _currentGameTime;

        public GameEngine()
        {
            _currentGameTime = new DateTime(2010, 1, 1);
        }

        public void Update()
        {
            _currentGameTime = _currentGameTime.AddSeconds(10);
            this.Publish(new GameTimeUpdated(_currentGameTime));
        }
    }
}