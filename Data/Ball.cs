using System;
using System.Numerics;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Data
{
    public class Ball : IBall
    {
        private Vector2 _Position;
        private readonly float _Radius;

        public override Vector2 Position => _Position;
        public override Vector2 Velocity { get; set; }
        public float Mass { get; private set; }
        public override float Radius => _Radius;

        private static readonly int MILISECONDS_PER_STEP = 10;
        private static readonly float STEP_SIZE = 0.1f;

        internal readonly IList<IObserver<IBall>> observers;

        public Ball(Vector2 position, Vector2 velocity)
        {
            observers = new List<IObserver<IBall>>();

            _Position = position;
            Velocity = velocity;
            Mass = 10.0F;
            _Radius = 2.0F;
        }

        public override async void StartMoving()
        {
            while (true)
            {
                Stopwatch watch = Stopwatch.StartNew();
                float steps = Velocity.Length() / STEP_SIZE;
                _Position += Vector2.Normalize(Velocity) * STEP_SIZE;
                foreach (IObserver<Ball> observer in observers)
                {
                    observer.OnNext(this);
                }
                watch.Stop();
                int delay = (int)(Convert.ToInt32(MILISECONDS_PER_STEP / steps) - watch.ElapsedMilliseconds);
                await Task.Delay(delay > 0 ? delay : 0);
            }
        }

        public override IDisposable Subscribe(IObserver<IBall> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly IList<IObserver<IBall>> _observers;
            private readonly IObserver<IBall> _observer;

            public Unsubscriber
            (IList<IObserver<IBall>> observers, IObserver<IBall> observer)
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
