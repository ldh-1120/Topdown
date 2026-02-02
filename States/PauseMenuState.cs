using System;
using Basement.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Basement.States {
    public class PauseMenuState : IGameState {
        private const float OVERLAY_MAX_ALPHA = 0.55f;
        private const float OVERLAY_FADE_DURATION = 0.25f;
        private const float UI_FADE_DURATION = 0.3f;
        private const float UI_SLIDE_PIXELS = 18f;
        private const float LINE_SPACING = 8f;

        private enum FadeMode { FadingIn, Idle, FadingOut }

        private readonly GameContext _context;
        private SpriteFont _font = default!;

        public bool AllowUpdateBelow => false;
        public bool AllowDrawBelow => true;

        private float _overlayTime;
        private float _uiTime;
        private FadeMode _mode;

        private bool _popQueued;

        public PauseMenuState(GameContext context) => _context = context;

        public void OnEnter() {
            _font = _context.Content.Load<SpriteFont>("Fonts/DefaultFont");
            _mode = FadeMode.FadingIn;

            _overlayTime = 0f;
            _uiTime = 0f;

            _popQueued = false;
        }

        public void OnExit() { }
        public void OnPause() { }
        public void OnResume() { }

        public bool HandleInput(GameTime gameTime) {
            if (_mode == FadeMode.FadingOut)
                return true;

            if (_context.Input.IsKeyPressed(Keys.Escape)) {
                _mode = FadeMode.FadingOut;
                return true;
            }
            return true;
        }

        public void Update(GameTime gameTime) {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_mode == FadeMode.FadingIn) {
                _overlayTime = MathHelper.Clamp(_overlayTime + delta / OVERLAY_FADE_DURATION, 0f, 1f);
                _uiTime = MathHelper.Clamp(_uiTime + delta / UI_FADE_DURATION, 0f, 1f);

                if (_overlayTime >= 1f && _uiTime >= 1f)
                    _mode = FadeMode.Idle;
            } else if (_mode == FadeMode.FadingOut) {
                _overlayTime = MathHelper.Clamp(_overlayTime - delta / OVERLAY_FADE_DURATION, 0f, 1f);
                _uiTime = MathHelper.Clamp(_uiTime - delta / UI_FADE_DURATION, 0f, 1f);

                if (_overlayTime <= 0f && _uiTime <= 0f && !_popQueued) {
                    _context.States.Pop();
                    _popQueued = true;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            Viewport viewport = _context.GraphicsDevice.Viewport;
            Rectangle rectangle = new Rectangle(0, 0, viewport.Width, viewport.Height);

            float overlayEase = MathHelper.SmoothStep(0f, 1f, _overlayTime);
            float uiEase = MathHelper.SmoothStep(0f, 1f, _uiTime);
            float slideOffsetY = UI_SLIDE_PIXELS * (1f - uiEase) * (_mode == FadeMode.FadingIn ? 1f : -1f);

            const string title = "PAUSED";
            const string hint = "Press ESC to Resume";
            Vector2 titleSize = _font.MeasureString(title);
            Vector2 hintSize = _font.MeasureString(hint);
            float totalHeight = titleSize.Y + hintSize.Y + LINE_SPACING;

            float centerX = viewport.Width * 0.5f;
            float centerY = viewport.Height * 0.5f;

            float startY = centerY - totalHeight * 0.5f;

            Vector2 titlePosition = new Vector2(centerX - titleSize.X * 0.5f, startY + slideOffsetY);
            Vector2 hintPosition = new Vector2(centerX - hintSize.X * 0.5f, startY + titleSize.Y + LINE_SPACING + slideOffsetY);

            spriteBatch.Begin();
            spriteBatch.Draw(_context.Render.Pixel, rectangle, Color.Black * overlayEase * OVERLAY_MAX_ALPHA);
            Color color = Color.White * uiEase;
            spriteBatch.DrawString(_font, title, titlePosition, color);
            spriteBatch.DrawString(_font, hint, hintPosition, color);
            spriteBatch.End();
        }
    }
}