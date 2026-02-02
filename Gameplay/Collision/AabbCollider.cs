using Basement.Core.Services;
using Basement.Gameplay.Components;
using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Collision {
    public sealed class AabbCollider : ICollider {

        public Point Size { get; }
        public Point Offset { get; set; }

        public AabbCollider(GameContext context = null, SpriteComponent sprite = null, int? width = null, int? height = null) {
            if (sprite is not null && context is not null) {
                var (atlas, _) = context.Assets.GetTextureAtlas(sprite.AtlasId);
                Rectangle sourceRect = atlas.GetRect(sprite.FrameName);
                width ??= sourceRect.Width;
                height ??= sourceRect.Height;
            }

            Size = new Point((int)(width.GetValueOrDefault(1) * (sprite?.Scale ?? 1)), (int)(height.GetValueOrDefault(1) * (sprite?.Scale ?? 1)));
        }

        public AabbF GetAabb(Vector2 worldPos) => new AabbF(worldPos.X + Offset.X, worldPos.Y + Offset.Y, Size.X, Size.Y);

        public CollisionManifold Collide(Vector2 position, ICollider other, Vector2 otherPos) => Collision.Dispatch(this, position, other, otherPos);
    }
}