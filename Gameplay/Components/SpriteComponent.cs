using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

namespace Basement.Gameplay.Components {
    public sealed class SpriteComponent: Component {
        public string AtlasId { get; }
        public string FrameName { get; set; }
        
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public float Rotation { get; set; } = 0f;
        public float Scale { get; set; } = 1f;
        public float Alpha { get; set; } = 1f;

        public SpriteComponent(string atlasId, string frameName) {
            AtlasId = atlasId;
            FrameName = frameName;
        }
    }
}