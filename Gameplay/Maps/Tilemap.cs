using System.Collections.Generic;
using Basement.Core.Assets;
using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Maps {
    public sealed class Tilemap {
        public TileProperties Properties { get; }
        public TilemapData Data { get; }
        public string TextureKey { get; }
        public Rectangle[] GidToSource { get; }

        public Tilemap(TilemapData data, string textureKey, Rectangle[] gidToSource, TileProperties properties) {
            Data = data;
            TextureKey = textureKey;
            GidToSource = gidToSource;
            Properties = properties;
        }

        public IEnumerable<TiledLayer> TileLayers() {
            for (int i = 0; i < Data.Layers.Count; i++) {
                if (Data.Layers[i].Type == "tilelayer" && Data.Layers[i].Data is not null)
                    yield return Data.Layers[i];
            }
        }

        public TiledLayer FindLayer(string name) {
            for (int i = 0; i < Data.Layers.Count; i++) {
                if (Data.Layers[i].Name == name)
                    return Data.Layers[i];
            }

            return null;
        }
    }
}