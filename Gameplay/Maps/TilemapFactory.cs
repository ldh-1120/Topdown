using Basement.Core.Assets;
using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Maps {
    public static class TilemapFactory {
        public static Tilemap BuildSingleTileset(TilemapData data, string textureKey) {
            if (data.Tilesets.Count == 0)
                throw new System.Exception("No tilesets found in tilemap data.");
            
            TiledTileset tileset = data.Tilesets[0];
            if (tileset.Image is null)
                throw new System.Exception("No tileset image found in tilemap data.");
            
            int firstGid = tileset.FirstGid;
            int tileCount = tileset.TileCount;
            int columns = tileset.Columns;

            int maxGid = firstGid + tileCount;
            Rectangle[] gidToRect = new Rectangle[maxGid];
            for (int local = 0; local < tileCount; local++) {
                int gid = firstGid + local;
                int xIndex = local % columns;
                int yIndex = local / columns;
                
                int x = xIndex * tileset.TileWidth;
                int y = yIndex * tileset.TileHeight;
                gidToRect[gid] = new Rectangle(x, y, tileset.TileWidth, tileset.TileHeight);
            }
            TileProperties properties = TileProperties.BuildFromSingleTileset(tileset);
            
            return new Tilemap(data, textureKey, gidToRect, properties);
        }
    }
}