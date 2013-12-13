using System;

namespace Life.Core.Basic
{
    public class GameTimeUpdated
    {
        public DateTime GameTime { get; private set; }

        public GameTimeUpdated(DateTime gameTime)
        {
            GameTime = gameTime;
        }
    }
}
