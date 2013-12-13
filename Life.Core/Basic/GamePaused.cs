namespace Life.Core.Basic
{
    public class GamePaused
    {
        public bool IsPaused { get; private set; }

        public GamePaused(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
}