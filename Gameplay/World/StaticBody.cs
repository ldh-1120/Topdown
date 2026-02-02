using Basement.Gameplay.Collision;
using Microsoft.Xna.Framework;

namespace Basement.Gameplay.World {
    public sealed class StaticBody {
        public Vector2 Position;
        public ICollider Collider;

        public StaticBody(Vector2 position, ICollider collider) {
            Position = position;
            Collider = collider;
        }

        public AabbF Aabb => Collider.GetAabb(Position);
    }
}