using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Data
{
    class Ball
    {
        private static int nextId = 0;

        public int id;
        public Vector2 position;
        public float mass;
        public float radius;
        public Vector2 velocity;

        public Ball(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }

        public void setVelocity(Vector2 newVelocity)
        {

        }

        public void updatePosition()
        {

        }
    }
}
