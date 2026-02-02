using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Collision {
    public readonly struct AabbF {
        public readonly float X, Y, W, H;

        public float Left => X;
        public float Top => Y;
        public float Right => X + W;
        public float Bottom => Y + H;

        public AabbF(float x, float y, float w, float h) => (X, Y, W, H) = (x, y, w, h);

        public bool Intersects(AabbF other) => Left < other.Right && Right > other.Left && Top < other.Bottom && Bottom > other.Top;

        public Rectangle ToRectangle() => new Rectangle((int)X, (int)Y, (int)W, (int)H);
    }
}