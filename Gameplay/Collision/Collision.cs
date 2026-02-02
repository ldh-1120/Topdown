using System;
using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Collision {
    public static class Collision {
        public static CollisionManifold Dispatch(AabbCollider a, Vector2 aPos, ICollider b, Vector2 bPos) {
            if (b is AabbCollider bb)
                return AabbVsAabb(a, aPos, bb, bPos);
            if (b is CircleCollider bc) 
                return AabbVsCircle(a, aPos, bc, bPos);
            return CollisionManifold.None;
        }

        public static CollisionManifold Dispatch(CircleCollider a, Vector2 aPos, ICollider b, Vector2 bPos) {
            if (b is CircleCollider bb) 
                return CircleVsCircle(a, aPos, bb, bPos);
            if (b is AabbCollider bb2) {
                CollisionManifold manifold = AabbVsCircle(bb2, bPos, a, aPos);
                if (!manifold.Hit) 
                    return manifold;
                return new CollisionManifold(true, -manifold.Normal, manifold.Penetration);
            }
            return CollisionManifold.None;
        }

        private static CollisionManifold AabbVsAabb(AabbCollider a, Vector2 aPos, AabbCollider b, Vector2 bPos) {
            AabbF ra = a.GetAabb(aPos);
            AabbF rb = b.GetAabb(bPos);

            if (!ra.Intersects(rb)) 
                return CollisionManifold.None;

            float left = rb.Right - ra.Left;
            float right = ra.Right - rb.Left;
            float top = rb.Bottom - ra.Top;
            float bottom = ra.Bottom - rb.Top;

            float penX = MathF.Min(left, right);
            float penY = MathF.Min(top, bottom);
            if (penX < penY) {
                Vector2 normal = (left < right) ? new Vector2(1f, 0f) : new Vector2(-1f, 0f);
                return new CollisionManifold(true, normal, penX);
            } else {
                Vector2 normal = (top < bottom) ? new Vector2(0f, 1f) : new Vector2(0f, -1f);
                return new CollisionManifold(true, normal, penY);
            }
        }

        private static CollisionManifold CircleVsCircle(CircleCollider a, Vector2 aPos, CircleCollider b, Vector2 bPos) {
            Vector2 ca = aPos + a.Offset;
            Vector2 cb = bPos + b.Offset;

            Vector2 vector = ca - cb;
            float distanceSquared = vector.LengthSquared();
            float radius = a.Radius + b.Radius;
            if (distanceSquared >= radius * radius)
                return CollisionManifold.None;

            float distance = MathF.Sqrt(MathF.Max(distanceSquared, 0.001f));
            Vector2 normal = vector / distance;
            float pen = radius - distance;

            return new CollisionManifold(true, normal, pen);
        }

        private static CollisionManifold AabbVsCircle(AabbCollider a, Vector2 aPos, CircleCollider c, Vector2 cPos) {
            AabbF ra = a.GetAabb(aPos);
            Vector2 center = cPos + c.Offset;

            float closestX = MathHelper.Clamp(center.X, ra.Left, ra.Right);
            float closestY = MathHelper.Clamp(center.Y, ra.Top, ra.Bottom);
            Vector2 closest = new Vector2(closestX, closestY);

            Vector2 vector = center - closest;
            float distanceSquared = vector.LengthSquared();

            if (distanceSquared > c.Radius * c.Radius)
                return CollisionManifold.None;

            float distance = MathF.Sqrt(MathF.Max(distanceSquared, 0.01f));
            Vector2 normal = (distance > 0f) ? (vector / distance) : new Vector2(0f, -1f);
            float pen = c.Radius - distance;

            return new CollisionManifold(true, -normal, pen);
        }
    }
}