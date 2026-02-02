using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Collision {
    public interface ICollider {
        AabbF GetAabb(Vector2 worldPos);

        CollisionManifold Collide(Vector2 position, ICollider other, Vector2 otherPos);
    }
}