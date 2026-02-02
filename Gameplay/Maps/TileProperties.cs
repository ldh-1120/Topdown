using System;
using System.Collections.Generic;
using System.Text.Json;
using Basement.Core.Assets;

namespace Basement.Gameplay.Maps {
    public sealed class TileProperties {
        private readonly Dictionary<int, Dictionary<string, object>> _byGid = new();

        public static TileProperties BuildFromSingleTileset(TiledTileset tileset) {
            TileProperties table = new();
            if (tileset.Tiles is null)
                return table;

            for (int i = 0; i < tileset.Tiles.Count; i++) {
                TiledTile tile = tileset.Tiles[i];
                int gid = tile.Id + tileset.FirstGid;
                if (tile.Properties is null || tile.Properties.Count == 0)
                    continue;

                Dictionary<string, object> dictionary = new(StringComparer.Ordinal);
                for (int p = 0; p < tile.Properties.Count; p++) {
                    TiledProperty property = tile.Properties[p];
                    if (property is null)
                        continue;

                    dictionary[property.Name] = property.Value;
                }
                table._byGid[gid] = dictionary;
            }

            return table;
        }

        public bool Has(int gid, string key) => _byGid.TryGetValue(gid, out Dictionary<string, object> dictionary) && dictionary.ContainsKey(key);

        public bool TryGet(int gid, string key, out object value) {
            value = default!;
            if (!_byGid.TryGetValue(gid, out Dictionary<string, object> dictionary))
                return false;

            return dictionary.TryGetValue(key, out value!);
        }

        public bool GetBool(int gid, string key, bool fallback = false) => TryGet(gid, key, out object value) ? Convert.ToBoolean(value) : fallback;

        public int GetInt(int gid, string key, int fallback = 0) => TryGet(gid, key, out object value) ? Convert.ToInt32(value) : fallback;

        public float GetFloat(int gid, string key, float fallback = 0f) => TryGet(gid, key, out object value) ? Convert.ToSingle(value) : fallback;

        public string GetString(int gid, string key, string fallback = "") => TryGet(gid, key, out object value) ? Convert.ToString(value) : fallback;

        private static object NormalizeValue(string type, object raw) {
            if (raw is JsonElement element) {
                return type switch {
                    "bool" => element.GetBoolean(),
                    "int" => element.GetInt32(),
                    "float" => (float)element.GetDouble(),
                    _ => element.GetString() ?? string.Empty,
                };
            }

            return raw;
        }
    }
}