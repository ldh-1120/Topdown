using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Basement.Core.Assets {
    public sealed class TilemapData {
        [JsonPropertyName("width")] public int Width { get; set; }
        [JsonPropertyName("height")] public int Height { get; set; }
        [JsonPropertyName("tilewidth")] public int TileWidth { get; set; }
        [JsonPropertyName("tileheight")] public int TileHeight { get; set; }

        [JsonPropertyName("tilesets")] public List<TiledTileset> Tilesets { get; set; }
        [JsonPropertyName("layers")] public List<TiledLayer> Layers { get; set; }
    }

    public sealed class TiledLayer {
        [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;
        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

#nullable enable
        [JsonPropertyName("data")] public int[]? Data { get; set; }
        [JsonPropertyName("width")] public int Width { get; set; }
        [JsonPropertyName("height")] public int Height { get; set; }

        [JsonPropertyName("objects")] public List<TiledObject>? Objects { get; set; }
    }

    public sealed class TiledObject {
        [JsonPropertyName("x")] public int X { get; set; }
        [JsonPropertyName("y")] public int Y { get; set; }
        [JsonPropertyName("width")] public int Width { get; set; }
        [JsonPropertyName("height")] public int Height { get; set; }
        
        [JsonPropertyName("rotation")] public int Rotation { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
        [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;
    }

    public sealed class TiledTileset {
        [JsonPropertyName("firstgid")] public int FirstGid { get; set; }
        
        [JsonPropertyName("image")] public string Image { get; set; } = string.Empty;
        [JsonPropertyName("imagewidth")] public int ImageWidth { get; set; }
        [JsonPropertyName("imageheight")] public int ImageHeight { get; set; }
        
        [JsonPropertyName("tilewidth")] public int TileWidth { get; set; }
        [JsonPropertyName("tileheight")] public int TileHeight { get; set; }
        [JsonPropertyName("columns")] public int Columns { get; set; }
        [JsonPropertyName("tilecount")] public int TileCount { get; set; }
        
        [JsonPropertyName("tiles")] public List<TiledTile>? Tiles { get; set; }
    }

    public sealed class TiledTile {
        [JsonPropertyName("id")] public int Id { get; set; }

        [JsonPropertyName("properties")] public List<TiledProperty>? Properties { get; set; }
    }

    public sealed class TiledProperty {
        [JsonPropertyName("name")] public string Name { get; set; } = "";
        [JsonPropertyName("type")] public string Type { get; set; } = "";
        [JsonPropertyName("value")] public object? Value { get; set; }
    }
}