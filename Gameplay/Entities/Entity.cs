using System;
using System.Collections.Generic;
using Basement.Gameplay.Components;
using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Entities {
    public abstract class Entity {
        public Vector2 Position;
        public Vector2 Velocity;
        public float z;
        public float zVelocity;

        private readonly List<Component> _components = new();

        public T Add<T>(T component) where T : Component {
            component.Owner = this;
            _components.Add(component);
            component.OnAdded();
            return component;
        }

#nullable enable
        public T? Get<T>() where T : Component {
            for (int i = 0; i < _components.Count; i++) {
                if (_components[i] is T typed)
                    return typed;
            }
            return null;
        }

        public virtual void Update(GameTime gameTime) {
            for (int i = 0; i < _components.Count; i++) {
                if (_components[i].Enabled)
                    _components[i].Update(gameTime);
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * deltaTime;
            zVelocity -= 25f * deltaTime;
            z += zVelocity;
            if (z < 0f) {
                z = 0f;
                zVelocity = 0f;
            }
        }
    }
}