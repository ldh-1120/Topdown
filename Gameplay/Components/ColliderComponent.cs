using Basement.Gameplay.Collision;
using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Components {
    public sealed class ColliderComponent : Component {
        public ICollider Collider { get; }

        public ColliderComponent(ICollider collider) => Collider = collider;

        public AabbF Aabb => Collider.GetAabb(Owner.Position);
    }
}