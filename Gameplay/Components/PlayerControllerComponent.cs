using Basement.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Basement.Gameplay.Components {
    public sealed class PlayerControllerComponent: Component {
        private readonly GameContext _context;

        private readonly MovementComponent _movement;
        private readonly AnimationComponent _animation;
        private readonly SpriteComponent _sprite;

        private const float MOVE_EPSILON_SQ = 0.01f;

        public PlayerControllerComponent(GameContext context, MovementComponent movement, AnimationComponent animation, SpriteComponent sprite) {
            _context = context;
            _movement = movement;
            _animation = animation;
            _sprite = sprite;
        }

        public override void Update(GameTime gameTime) {
            Vector2 velocity = _movement.Owner.Velocity;
            if (velocity.LengthSquared() > MOVE_EPSILON_SQ)
                _animation.Play("run", false, true);
            else 
                _animation.Play("idle", false, true);

            if (velocity.X < 0f)
                _sprite.Effects = SpriteEffects.FlipHorizontally;
            else if (velocity.X > 0f)
                _sprite.Effects = SpriteEffects.None;
        }
    }
}