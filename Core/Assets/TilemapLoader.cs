using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;

namespace Basement.Core.Assets {
    public static class TilemapLoader {
        public static TilemapData Load(string jsonPath) {
            using Stream stream = TitleContainer.OpenStream(jsonPath);
            using StreamReader reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            
            JsonSerializerOptions options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };
            TilemapData data = JsonSerializer.Deserialize<TilemapData>(json, options);
            if (data is null)
                throw new InvalidDataException($"Failed to load tilemap data from {jsonPath}");
                
            return data;
        }
    }
}