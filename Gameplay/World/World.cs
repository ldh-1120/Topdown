using System.Collections.Generic;
using Basement.Core.Services;
using Basement.Gameplay.Collision;
using Basement.Gameplay.Components;
using Basement.Gameplay.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Basement.Gameplay.World {
    public sealed class World {
        private readonly List<Entity> _entities = new();
        private readonly List<StaticBody> _solids = new();

        public IEnumerable<Entity> Entities => _entities;
        public IEnumerable<StaticBody> Solids => _solids;

        public void Add(Entity entity) => _entities.Add(entity);
        public void AddSolid(StaticBody body) => _solids.Add(body);

        public void Update(GameTime gameTime) {
            for (int i = 0; i < _entities.Count; i++)
                _entities[i].Update(gameTime);
        }

        public void QuerySolids(AabbF aabb, List<StaticBody> results) {
            results.Clear();
            for (int i = 0; i < _solids.Count; i++) {
                if (_solids[i].Aabb.Intersects(aabb))
                    results.Add(_solids[i]);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameContext context, bool debug = true) {
            foreach (Entity entity in _entities) {
                SpriteComponent sprite = entity.Get<SpriteComponent>();
                if (sprite is null)
                    continue;

                var (atlas, texture) = context.Assets.GetTextureAtlas(sprite.AtlasId);
                Rectangle sourceRect = atlas.GetRect(sprite.FrameName);
                Vector2 position = new Vector2(entity.Position.X, entity.Position.Y - entity.z);
                spriteBatch.Draw(texture, position, sourceRect, Color.White * sprite.Alpha, sprite.Rotation, sprite.Origin, sprite.Scale, sprite.Effects, 0f);
            }

            foreach (StaticBody body in _solids) {
                ICollider collider = body.Collider;
                if (collider is AabbCollider aabb) {
                    AabbF rect = aabb.GetAabb(body.Position); 
                    DrawRect(spriteBatch, context.Render.Pixel, rect.ToRectangle(), 1, Color.Green);
                }
            }

            if (!debug)
                return;
            
            foreach (Entity entity in _entities) {
                ColliderComponent collider = entity.Get<ColliderComponent>();
                AabbF rect = collider.Collider.GetAabb(entity.Position);
                DrawRect(spriteBatch, context.Render.Pixel, rect.ToRectangle(), 1, Color.Red);
            }
        }

        private static void DrawRect(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rect, int thickness, Color color) {
            spriteBatch.Draw(pixel, new Rectangle(rect.Left, rect.Top, rect.Width, thickness), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.Left, rect.Bottom - thickness, rect.Width, thickness), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.Left, rect.Top, thickness, rect.Height), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.Right - thickness, rect.Top, thickness, rect.Height), color);
        }
    }
}