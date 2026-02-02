using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Collision {
    public sealed class CircleCollider : ICollider {
        public float Radius { get; }
        public Vector2 Offset { get; set; }

        public CircleCollider(float radius) {
            Radius = radius;
            Offset = Vector2.Zero;
        }

        public AabbF GetAabb(Vector2 worldPos) {
            Vector2 center = worldPos + Offset;
            float diameter = Radius * 2;
            return new AabbF(center.X - Radius, center.Y - Radius, diameter, diameter);
        }

        public CollisionManifold Collide(Vector2 position, ICollider other, Vector2 otherPos) => Collision.Dispatch(this, position, other, otherPos);
    }
}