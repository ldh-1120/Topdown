using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Basement.Core.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Basement.Core.Services {
    public class AssetService {
        private readonly ContentManager _content;

        private readonly Dictionary<string, Texture2D> _textures = new(StringComparer.Ordinal);
        private readonly Dictionary<string, TextureAtlas> _atlases = new(StringComparer.Ordinal);

        private readonly Dictionary<string, (string jsonPath, string textureKey)> _atlasRegistry = new(StringComparer.Ordinal);

        public AssetService(ContentManager content) => _content = content;

        public void RegisterTextureAtlas(string atlasId, string jsonPath, string textureKey) => _atlasRegistry[atlasId] = (jsonPath, textureKey);

        public Texture2D GetTexture(string key) {
            if (_textures.TryGetValue(key, out Texture2D texture))
                return texture;

            texture = _content.Load<Texture2D>(key);
            _textures[key] = texture;
            return texture;
        }

        public (TextureAtlas atlas, Texture2D texture) GetTextureAtlas(string atlasId) {
            if (!_atlasRegistry.TryGetValue(atlasId, out var registry))
                throw new KeyNotFoundException($"Texture atlas '{atlasId}' is not registered.");

            TextureAtlas atlas = GetOrLoadAtlas(atlasId, registry.jsonPath);
            Texture2D texture = GetTexture(registry.textureKey);
            return (atlas, texture);
        }

        private TextureAtlas GetOrLoadAtlas(string atlasId, string jsonPath) {
            if (_atlases.TryGetValue(atlasId, out TextureAtlas atlas))
                return atlas;

            string jsonText;
            using (var stream = TitleContainer.OpenStream(jsonPath))
            using (var reader = new StreamReader(stream))
                jsonText = reader.ReadToEnd();

            atlas = TextureAtlas.FromJson(jsonText);
            _atlases[atlasId] = atlas;
            return atlas;
        }
    }
}