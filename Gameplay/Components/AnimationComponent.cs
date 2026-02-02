using System.Collections.Generic;
using Basement.Core.Services;
using Microsoft.Xna.Framework;
using static Basement.Core.Assets.TextureAtlas;

namespace Basement.Gameplay.Components {
    public sealed class AnimationComponent: Component {
        private readonly GameContext _context;
        private readonly SpriteComponent _sprite;

        public string CurrentTag { get; private set; } = string.Empty;
        public bool Loop { get; set; } = true;

        private int _frameIndex;
        private float _accumulatedMs;
        private bool _playing;

        public AnimationComponent(GameContext context, SpriteComponent sprite) {
            _context = context;
            _sprite = sprite;
        }

        public void Play(string tagName, bool restart = false, bool loop = true) {
            var (atlas, _) = _context.Assets.GetTextureAtlas(_sprite.AtlasId);

            if (!atlas.TryGetTag(tagName, out TagInfo tagInfo))
                throw new KeyNotFoundException($"Tag '{tagName}' not found.");
            
            Loop = loop;

            if (!restart && _playing && CurrentTag == tagName)
                return;

            CurrentTag = tagName;
            _playing = true;
            _accumulatedMs = 0f;

            _frameIndex = tagInfo.Direction == "reverse" ? tagInfo.To : tagInfo.From;
            _sprite.FrameName = atlas.GetFrameNameAt(_frameIndex);
        }

        public void Stop() {
            _playing = false;
            _accumulatedMs = 0f;
        }

        public override void Update(GameTime gameTime) {
            if (!_playing)
                return;

            var (atlas, _) = _context.Assets.GetTextureAtlas(_sprite.AtlasId);
            TagInfo tag = atlas.GetTag(CurrentTag);

            float deltaMs = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _accumulatedMs += deltaMs;

            string currentName = atlas.GetFrameNameAt(_frameIndex);
            int durationMs = atlas.GetFrame(currentName).DurationMs;

            while (_accumulatedMs >= durationMs && _playing) {
                _accumulatedMs -= durationMs;
                Advance(tag);

                currentName = atlas.GetFrameNameAt(_frameIndex);
                durationMs = atlas.GetFrame(currentName).DurationMs;
            }

            _sprite.FrameName = currentName;
        }

        private void Advance(TagInfo tag) {
            if (tag.Direction == "reverse") {
                _frameIndex--;
                if (_frameIndex < tag.From) {
                    if (Loop)
                        _frameIndex = tag.To;
                    else {
                        _frameIndex = tag.From;
                        _playing = false;
                    }
                }
            } else {
                _frameIndex++;
                if (_frameIndex > tag.To) {
                    if (Loop)
                        _frameIndex = tag.From;
                    else {
                        _frameIndex = tag.To;
                        _playing = false;
                    }
                }
            }
        }
    }
}