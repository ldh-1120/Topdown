using Microsoft.Xna.Framework.Input;

namespace Basement.Engine.Input {
    public class InputState {
        private KeyboardState _prev;
        private KeyboardState _current;

        public void Update() {
            _prev = _current;
            _current = Keyboard.GetState();
        }

        public bool IsKeyPressed(Keys key) => _current.IsKeyDown(key) && !_prev.IsKeyDown(key);

        public bool IsKeyReleased(Keys key) => !_current.IsKeyDown(key) && _prev.IsKeyDown(key);

        public bool IsKeyDown(Keys key) => _current.IsKeyDown(key);
    }
}