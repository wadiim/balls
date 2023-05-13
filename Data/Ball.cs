using System;
using System.Numerics;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Data
{
    public class Ball : IObservable<Ball>
    {
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; set; }
        public float Mass { get; private set; }
        public float Radius { get; private set; }

        private static readonly int MILISECONDS_PER_STEP = 10;
        private static readonly float STEP_SIZE = 0.1f;

        internal readonly IList<IObserver<Ball>> observers;

        public Ball(Vector2 position, Vector2 velocity)
        {
            observers = new List<IObserver<Ball>>();

            Position = position;
            Velocity = velocity;
            Mass = 10.0F;
            Radius = 2.0F;
        }

        public async void StartMoving()
        {
            while (true)
            {
                Stopwatch watch = Stopwatch.StartNew();
                float steps = Velocity.Length() / STEP_SIZE;
                Position += Vector2.Normalize(Velocity) * STEP_SIZE;
                foreach (IObserver<Ball> observer in observers)
                {
                    observer.OnNext(this);
                }
                watch.Stop();
                int delay = (int)(Convert.ToInt32(MILISECONDS_PER_STEP / steps) - watch.ElapsedMilliseconds);
                await Task.Delay(delay > 0 ? delay : 0);
            }
        }

        public IDisposable Subscribe(IObserver<Ball> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly IList<IObserver<Ball>> _observers;
            private readonly IObserver<Ball> _observer;

            public Unsubscriber
            (IList<IObserver<Ball>> observers, IObserver<Ball> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
