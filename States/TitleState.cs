using Basement.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Basement.States {
    public class TitleState: IGameState {
        private readonly GameContext _context;
        private SpriteFont _font;

        public TitleState(GameContext context) => _context = context;

        public bool AllowDrawBelow => false;
        public bool AllowUpdateBelow => false;

        public void OnEnter() {
            _font = _context.Content.Load<SpriteFont>("Fonts/DefaultFont");
        }

        public void OnExit() {}
        public void OnPause() {}
        public void OnResume() {}

        public bool HandleInput(GameTime gameTime) {
            if (_context.Input.IsKeyPressed(Keys.Enter)) {
                _context.States.Replace(new GameplayState(_context));
                return true;
            }
            return false;
        }

        public void Update(GameTime gameTime) {}

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin();
            spriteBatch.DrawString(_font, "BASEMENT\nPress Enter to Start", new Vector2(40, 40), Color.White);
            spriteBatch.End();
        }
    }
}