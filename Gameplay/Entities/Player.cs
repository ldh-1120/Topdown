using Basement.Gameplay.Components;
using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Entities {
    public sealed class Player: Entity {
        public const int Size = 16;

        public Player(Vector2 startPosition) {
            Position = startPosition;
            z = 100f;
        }
    }
}