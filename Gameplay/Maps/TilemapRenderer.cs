using Basement.Core.Assets;
using Basement.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Basement.Gameplay.Maps {
    public sealed class TilemapRenderer {
        public void Draw(SpriteBatch spriteBatch, GameContext context, Tilemap map) {
            Texture2D texture = context.Assets.GetTexture(map.TextureKey);
            int tileWidth = map.Data.TileWidth;
            int tileHeight = map.Data.TileHeight;
            foreach (TiledLayer layer in map.TileLayers()) {
                int[] data = layer.Data;
                int width = layer.Width;
                int height = layer.Height;
                for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    int idx = y * width + x;

                    int gid = data[idx];
                    if (gid == 0) 
                        continue;

                    if ((uint)gid >= (uint)map.GidToSource.Length)
                        continue;
                    
                    Rectangle source = map.GidToSource[gid];
                    Vector2 position = new Vector2(x * tileWidth, y * tileHeight);

                    spriteBatch.Draw(texture, position, source, Color.White);
                }
            }
        }
    }
}