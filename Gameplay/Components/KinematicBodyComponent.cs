using System.Collections.Generic;
using Basement.Gameplay.Collision;
using Basement.Gameplay.World;
using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Components {
    public sealed class KinematicBodyComponent : Component {
        private readonly World.World _world;
        private readonly ICollider _collider;

        private const float SKIN = 0f;
        private const float PEN_EPSILON = 0.01f;
        private const int MAX_ITERATIONS = 3;

        private readonly List<StaticBody> _candidates = new();

        public KinematicBodyComponent(World.World world, ColliderComponent collider) {
            _world = world;
            _collider = collider.Collider;
        }

        public override void Update(GameTime gameTime) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 velocity = Owner.Velocity;
            Vector2 position = Owner.Position;
            Vector2 target = position + velocity * deltaTime;

            AabbF aabb = _collider.GetAabb(position);
            _world.QuerySolids(aabb, _candidates);
            for (int iter = 0; iter < MAX_ITERATIONS; iter++) {
                bool anyHit = false;
                for (int i = 0; i < _candidates.Count; i++) {
                    StaticBody body = _candidates[i];
                    CollisionManifold  manifold = _collider.Collide(target, body.Collider, body.Position);
                    if (!manifold.Hit || manifold.Penetration < PEN_EPSILON)
                        continue;

                    anyHit = true;
                    target += manifold.Normal * (manifold.Penetration + SKIN);

                    float velocityNormal = Vector2.Dot(velocity, manifold.Normal);
                    if (velocityNormal < 0f)
                        velocity -= manifold.Normal * velocityNormal;
                }

                if (!anyHit)
                    break;
            }

            Owner.Position = target;
            Owner.Velocity = velocity;
        }
    }
}