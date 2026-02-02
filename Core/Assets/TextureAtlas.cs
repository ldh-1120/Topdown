using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Xna.Framework;

namespace Basement.Core.Assets {
    public sealed class TextureAtlas {
        public sealed record FrameInfo(Rectangle Rect, int DurationMs);
        public sealed record TagInfo(string Name, int From, int To, string Direction);
        
        public string ImageFileName { get; }
        private readonly Dictionary<string, FrameInfo> _framesByName;
        private readonly List<string> _frameNamesInOrder;
        private readonly Dictionary<string, TagInfo> _tagsByName;

        private TextureAtlas(string imageFileName, Dictionary<string, FrameInfo> framesByName, List<string> frameNamesInOrder, Dictionary<string, TagInfo> tagsByName) {
            ImageFileName = imageFileName;
            _framesByName = framesByName;
            _frameNamesInOrder = frameNamesInOrder;
            _tagsByName = tagsByName;
        }

        public Rectangle GetRect(string frameName) => GetFrame(frameName).Rect;

        public FrameInfo GetFrame(string frameName) {
            if (!_framesByName.TryGetValue(frameName, out FrameInfo info))
                throw new KeyNotFoundException($"Frame '{frameName}' not found.");
            return info;
        }

        public string GetFrameNameAt(int index) {
            if ((uint)index >= (uint)_frameNamesInOrder.Count)
                throw new IndexOutOfRangeException($"Index {index} is out of range.");
            return _frameNamesInOrder[index];
        }

        public int FrameCount => _frameNamesInOrder.Count;

        public TagInfo GetTag(string tagName) {
            if (!_tagsByName.TryGetValue(tagName, out TagInfo info))
                throw new KeyNotFoundException($"Tag '{tagName}' not found.");
            return info;
        }

        public bool TryGetTag(string tagName, out TagInfo info) => _tagsByName.TryGetValue(tagName, out info);

        public static TextureAtlas FromJson(string jsonText) {
            using JsonDocument document = JsonDocument.Parse(jsonText);
            JsonElement root = document.RootElement;

            string image = root.GetProperty("meta").GetProperty("image").GetString() ?? throw new FormatException("Image file name not found.");
            JsonElement framesElement = root.GetProperty("frames");
            Dictionary<string, FrameInfo> framesByName = new Dictionary<string, FrameInfo>(StringComparer.Ordinal);
            List<string> frameNamesInOrder = new List<string>(capacity:256);
            if (framesElement.ValueKind == JsonValueKind.Array) {
                foreach (JsonElement item in framesElement.EnumerateArray()) {
                    string name = item.GetProperty("filename").GetString() ?? throw new FormatException("Frame name not found.");
                    Rectangle rect = ReadRect(item.GetProperty("frame"));
                    int duration = item.TryGetProperty("duration", out JsonElement durationElement) ? durationElement.GetInt32() : 100;

                    framesByName[name] = new FrameInfo(rect, duration);
                    frameNamesInOrder.Add(name);
                }
            } else if (framesElement.ValueKind == JsonValueKind.Object) {
                foreach (var item in framesElement.EnumerateObject()) {
                    string name = item.Name;
                    Rectangle rect = ReadRect(item.Value.GetProperty("frame"));
                    int duration = item.Value.TryGetProperty("duration", out JsonElement durationElement) ? durationElement.GetInt32() : 100;

                    framesByName[name] = new FrameInfo(rect, duration);
                    frameNamesInOrder.Add(name);
                }
            } else
                throw new FormatException("Frames element is not an array or object."); 

            Dictionary<string, TagInfo> tagsByName = new Dictionary<string, TagInfo>(StringComparer.Ordinal);
            if (root.GetProperty("meta").TryGetProperty("frameTags", out JsonElement tagsElement) && tagsElement.ValueKind == JsonValueKind.Array) {
                foreach (JsonElement item in tagsElement.EnumerateArray()) {
                    string name = item.GetProperty("name").GetString() ?? "tag";
                    int from = item.GetProperty("from").GetInt32();
                    int to = item.GetProperty("to").GetInt32();
                    string direction = item.TryGetProperty("duration", out JsonElement directionElement) ? (directionElement.GetString() ?? "forward") : "forward";

                    tagsByName[name] = new TagInfo(name, from, to, direction); 
                }
            }

            return new TextureAtlas(image, framesByName, frameNamesInOrder, tagsByName);
        }

        private static Rectangle ReadRect(JsonElement frameRect) {
            int x = frameRect.GetProperty("x").GetInt32();
            int y = frameRect.GetProperty("y").GetInt32();
            int w = frameRect.GetProperty("w").GetInt32();
            int h = frameRect.GetProperty("h").GetInt32();
            return new Rectangle(x, y, w, h);
        }
    }
}