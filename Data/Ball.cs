using System.Numerics;

namespace Data
{
    public class Ball
    {
        public int Id { get; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; set; }
        public float Mass { get; private set; }
        public float Radius { get; private set; }

        public Ball(int id, Vector2 position, Vector2 velocity)
        {
            Id = id;
            Position = position;
            Velocity = velocity;
            Mass = 10.0F;
            Radius = 10.0F;
        }

        public void UpdatePosition()
        {
            Position += Velocity;
        }
    }
}
