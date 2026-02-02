using Basement.Gameplay.Entities;
using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Components {
    public abstract class Component {
        public bool Enabled { get; set; } = true;
        public Entity Owner { get; internal set; } = default!;

        public virtual void OnAdded() { }
        public virtual void Update(GameTime gameTime) { }
    }
}