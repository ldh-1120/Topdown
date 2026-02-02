using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Basement.States {
    public interface IGameState {
        void OnEnter();
        void OnExit();
        void OnResume();
        void OnPause();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        bool HandleInput(GameTime gameTime);
        
        bool AllowUpdateBelow { get; }
        bool AllowDrawBelow { get; }
    }
}