using System;
using System.Threading;

using Life.Core.Caliburn.Micro;

namespace Life.Core.Basic
{
    public class MainLoop
    {
        #region Fields

        private const double DefaultDelay = 1024;
        private const double MaxDelay = 4096;
        private const double MinDelay = 16;

        private readonly GameEngine _engine;
        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _pauseResetEvent = new ManualResetEvent(false);

        private double _delay;
        private bool _isPaused;
        private bool _stopping;

        #endregion Fields

        #region Constructors

        public MainLoop(GameEngine engine)
        {
            _engine = engine;
            Delay = DefaultDelay;
            IsPaused = false;
        }

        #endregion Constructors

        #region Public Properties

        public double Delay
        {
            get
            {
                return _delay;
            }

            private set
            {
                _delay = value;
                _resetEvent.Set();
                this.Publish(new PropertyChanged(this, () => Delay));
            }
        }

        public bool IsPaused
        {
            get
            {
                return _isPaused;
            }

            private set
            {
                _isPaused = value;
                _resetEvent.Set();
                this.Publish(new GamePaused(_isPaused));
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void DecreaseSpeed()
        {
            if (Delay * 2 > MaxDelay) return;
            Delay *= 2;
        }

        public void IncreaseSpeed()
        {
            if (Delay / 2 < MinDelay) return;
            Delay /= 2;
        }

        public void Start()
        {
            var gameThread = new Thread(StartThread);
            gameThread.Start();
        }

        public void Stop()
        {
            _pauseResetEvent.Set();
            _stopping = true;
        }

        public void TogglePause()
        {
            IsPaused = !IsPaused;
            if (!IsPaused) _pauseResetEvent.Set();
        }

        #endregion Public Methods

        #region Private Methods

        private void StartThread()
        {
            var previousTick = DateTime.Now;
            do
            {
                _pauseResetEvent.Reset();
                if (IsPaused) _pauseResetEvent.WaitOne();
                if (_stopping) break;

                var eventWasSet = false;
                if (previousTick + TimeSpan.FromMilliseconds(Delay) > DateTime.Now)
                {
                    _resetEvent.Reset();
                    var sleepTime = (previousTick.AddMilliseconds(Delay) - DateTime.Now).TotalMilliseconds;
                    sleepTime = Math.Max(sleepTime, 0);
                    eventWasSet = _resetEvent.WaitOne((int)sleepTime);
                }

                if (eventWasSet) continue;

                _engine.Update();
                previousTick = DateTime.Now;
            } while (!_stopping);
        }

        #endregion Private Methods
    }
}