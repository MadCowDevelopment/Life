using System;
using System.Collections.Generic;
using System.Linq;
using Life.Core.Caliburn.Micro;

namespace Life.Core.Basic
{
    public class GameEngine
    {
        private readonly IGameEnvironment _environment;
        private List<IGameEntity> _entities;

        public GameEngine(IGameEnvironment environment)
        {
            _environment = environment;
        }

        public void Initialize(IEnumerable<IGameEntity> entities)
        {
            _entities = entities.ToList();
        }

        public void Update()
        {
            _environment.Update();
            _entities.ForEach(p => p.Update());
        }
    }

    public interface IGameEntity
    {
        void Update();

    }

    public class GameEnvironment : IGameEnvironment
    {
        public GameEnvironment()
        {
            CurrentGameTime = new DateTime(2010, 1, 1);
        }

        public DateTime CurrentGameTime
        {
            get;
            private set;
        }

        public void Update()
        {
            CurrentGameTime = CurrentGameTime.AddSeconds(10);
            this.Publish(new GameTimeUpdated(CurrentGameTime));
        }
    }

    public abstract class GameEntity : IGameEntity
    {
        protected GameEntity(IGameEnvironment environment)
        {
            Environment = environment;
        }

        protected IGameEnvironment Environment { get; private set; }
        public abstract void Update();
    }

    public interface IGameEnvironment
    {
        void Update();

        DateTime CurrentGameTime { get; }
    }

    public class Human : GameEntity
    {
        public Human(IGameEnvironment environment, DateTime birthday) : base(environment)
        {
            Birthday = birthday;
        }

        public DateTime Birthday { get; private set; }

        public TimeSpan Age
        {
            get
            {
                return Environment.CurrentGameTime - Birthday;
            }
        }

        public override void Update()
        {
            this.Publish(new PropertyChanged(this, () => Age));
        }
    }
}