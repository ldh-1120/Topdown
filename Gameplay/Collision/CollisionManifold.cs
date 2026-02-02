using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Collision {
    public class CollisionManifold {
        public readonly bool Hit;
        public readonly Vector2 Normal;
        public readonly float Penetration;

        public CollisionManifold(bool hit, Vector2 normal, float penetration) {
            Hit = hit;
            Normal = normal;
            Penetration = penetration;
        }

        public static CollisionManifold None => new(false, Vector2.Zero, 0f);
    }
}