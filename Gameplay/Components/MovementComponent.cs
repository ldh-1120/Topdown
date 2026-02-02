using Basement.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Basement.Gameplay.Components {
    public sealed class MovementComponent: Component {
        private readonly InputState _input;

        public float Speed { get; set; } = 100f;

        public MovementComponent(InputState input) {
            _input = input;
        }

        public override void Update(GameTime gameTime) {
            Vector2 direction = Vector2.Zero;
            if (_input.IsKeyDown(Keys.W))
                direction.Y -= 1f;
            if (_input.IsKeyDown(Keys.S))
                direction.Y += 1f;
            if (_input.IsKeyDown(Keys.A))
                direction.X -= 1f;
            if (_input.IsKeyDown(Keys.D))
                direction.X += 1f; 
            if (_input.IsKeyPressed(Keys.Space))
                Owner.zVelocity += 10f; 

            if (direction != Vector2.Zero)
                direction.Normalize();
            Owner.Velocity = direction * Speed;
        }
    }
}