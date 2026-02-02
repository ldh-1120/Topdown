using Basement.Core.Services;
using Basement.Gameplay.Collision;
using Basement.Gameplay.Components;
using Basement.Gameplay.Entities;
using Basement.Gameplay.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Basement.States {
    public class GameplayState: IGameState {
        private readonly GameContext _context;
        private SpriteFont _font = default!;
        private World _world = default!;

        public bool AllowUpdateBelow => false;
        public bool AllowDrawBelow => false;

        public GameplayState(GameContext context) => _context = context;

        public void OnEnter() {
            _font = _context.Content.Load<SpriteFont>("Fonts/DefaultFont");
            _world = new World();

            _world.AddSolid(new StaticBody(new Vector2(0f, 0f), new AabbCollider(300, 20) { Offset = new Point(100, 100) }));

            Viewport viewport = _context.GraphicsDevice.Viewport;
            Vector2 startPosition = new Vector2(viewport.Width / 2f, viewport.Height / 2f);

            SpriteComponent sprite = new SpriteComponent("Character", "idle_0") { Scale = 4f };
            AnimationComponent animation = new AnimationComponent(_context, sprite);
            CircleCollider circle = new CircleCollider(16f) { Offset = new Vector2(8f, 8f) };
            ColliderComponent collider = new ColliderComponent(circle);
            KinematicBodyComponent body = new KinematicBodyComponent(_world, collider);
            MovementComponent movement = new MovementComponent(_context.Input) { Speed = 200f };
            PlayerControllerComponent playerController = new PlayerControllerComponent(_context, movement, animation, sprite);

            Player player = new Player(startPosition);
            player.Add(sprite);
            player.Add(animation);
            player.Add(collider);
            player.Add(body);
            player.Add(movement);
            player.Add(playerController);
            animation.Play("idle", true, true);

            _world.Add(player);
        }

        public void OnExit() {}
        public void OnPause() {}
        public void OnResume() {}

        public bool HandleInput(GameTime gameTime) {
            if (_context.Input.IsKeyPressed(Keys.Escape)) {
                _context.States.Push(new PauseMenuState(_context));
                return true;
            }
            return false;
        }

        public void Update(GameTime gameTime) {
            _world.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            _world.Draw(spriteBatch, _context, true);
            spriteBatch.DrawString(_font, "WASD to Move - Press ESC to Pause", new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
    }
}