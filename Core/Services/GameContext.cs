using Basement.Engine.Input;
using Basement.States;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Basement.Core.Services {
    public class GameContext {
        public required InputState Input { get; init; }
        public required StateStack States { get; init; }
        public required GraphicsDevice GraphicsDevice { get; init; }
        public required ContentManager Content { get; init; }
        public required RenderService Render { get; init; }
        public required AssetService Assets { get; init; }
    }
}