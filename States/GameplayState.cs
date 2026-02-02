using Basement.Core.Assets;
using Basement.Core.Rendering;
using Basement.Core.Services;
using Basement.Engine.Input;
using Basement.Gameplay.Collision;
using Basement.Gameplay.Components;
using Basement.Gameplay.Entities;
using Basement.Gameplay.Maps;
using Basement.Gameplay.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Basement.States {
    public class GameplayState: IGameState {
        private readonly GameContext _context;
        private SpriteFont _font = default!;
        private World _world = default!;
        private Tilemap _map = default!;
        private TilemapRenderer _mapRenderer = new();
        private Camera _camera = default!;
        private Player _player = default!;

        public bool AllowUpdateBelow => false;
        public bool AllowDrawBelow => false;

        public GameplayState(GameContext context) => _context = context;

        public void OnEnter() {
            _font = _context.Content.Load<SpriteFont>("Fonts/DefaultFont");
            TilemapData data = TilemapLoader.Load("Content/Maps/testmap.json");
            _map = TilemapFactory.BuildSingleTileset(data, "Tilesets/tileset");

            _world = new World();

            _camera = new Camera(_context.GraphicsDevice.Viewport.Bounds);
            _camera.SetZoom(4f);

            TiledLayer collision = _map.FindLayer("Collision");
            if (collision is not null)
                TileCollisionBuilder.BuildFromObjectLayer(_world, collision);

            Viewport viewport = _context.GraphicsDevice.Viewport;
            Vector2 startPosition = new Vector2(0f, 0f);

            SpriteComponent sprite = new SpriteComponent("Character", "idle_0");
            AnimationComponent animation = new AnimationComponent(_context, sprite);
            AabbCollider aabb = new AabbCollider(_context, sprite);
            ColliderComponent collider = new ColliderComponent(aabb);
            KinematicBodyComponent body = new KinematicBodyComponent(_world, collider);
            MovementComponent movement = new MovementComponent(_context.Input) { Speed = 50f };
            PlayerControllerComponent playerController = new PlayerControllerComponent(_context, movement, animation, sprite);

            _player = new Player(startPosition);
            _player.Add(sprite);
            _player.Add(animation);
            _player.Add(playerController);
            _player.Add(collider);
            _player.Add(movement);
            _player.Add(body);
            animation.Play("idle", true, true);

            _world.Add(_player);
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

            if (_context.Input.IsMouseDown(MouseButton.Left))
                _camera.AddShake(2f, 1f);

            _camera.Follow(_player.Position);
            _camera.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());
            _mapRenderer.Draw(spriteBatch, _context, _map);
            _world.Draw(spriteBatch, _context, true);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.DrawString(_font, "WASD to Move - Press ESC to Pause", new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
    }
}