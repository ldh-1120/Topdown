using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Collision {
    public sealed class AabbCollider : ICollider {
        public Point Size { get; }
        public Point Offset { get; set; }

        public AabbCollider(int width, int height) {
            Size = new Point(width, height);
            Offset = new Point(-width / 2, -height / 2);
        }

        public AabbF GetAabb(Vector2 worldPos) => new AabbF(worldPos.X + Offset.X, worldPos.Y + Offset.Y, Size.X, Size.Y);

        public CollisionManifold Collide(Vector2 position, ICollider other, Vector2 otherPos) => Collision.Dispatch(this, position, other, otherPos);
    }
}