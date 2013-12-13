using System;
using Caliburn.Micro;

namespace Life.Core.Caliburn.Micro
{
    public static class ObjectExtensions
    {
        private static volatile object _syncLock = new object();

        private static IEventAggregator _instance;

        public static IEventAggregator Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EventAggregator();
                        }
                    }
                }

                return _instance;
            }

            set
            {
                _instance = value;
            }
        }

        public static bool HandlerExistsFor(this object obj, Type messageType)
        {
            return Instance.HandlerExistsFor(messageType);
        }

        public static void Subscribe(this object obj)
        {
            Instance.Subscribe(obj);
        }

        public static void Unsubscribe(this object obj)
        {
            Instance.Unsubscribe(obj);
        }

        public static void Publish(this object obj, object message)
        {
            Instance.Publish(message);
        }

        public static void Publish(this object obj, object message, Action<Action> marshal)
        {
            Instance.Publish(message, marshal);
        }
    }
}
