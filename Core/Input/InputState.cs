using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Basement.Engine.Input {
    public class InputState {
        private KeyboardState _prevKeyboard;
        private KeyboardState _currentKeyboard;

        private MouseState _prevMouse;
        private MouseState _currentMouse;

        public void Update() {
            _prevKeyboard = _currentKeyboard;
            _currentKeyboard = Keyboard.GetState();

            _prevMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
        }

        public bool IsKeyPressed(Keys key) => _currentKeyboard.IsKeyDown(key) && !_prevKeyboard.IsKeyDown(key);
        public bool IsKeyReleased(Keys key) => !_currentKeyboard.IsKeyDown(key) && _prevKeyboard.IsKeyDown(key);
        public bool IsKeyDown(Keys key) => _currentKeyboard.IsKeyDown(key);

        public bool IsMousePressed(MouseButton button) => GetButtonState(_currentMouse, button) == ButtonState.Pressed && GetButtonState(_prevMouse, button) == ButtonState.Released;
        public bool IsMouseReleased(MouseButton button) => GetButtonState(_currentMouse, button) == ButtonState.Released && GetButtonState(_prevMouse, button) == ButtonState.Pressed;
        public bool IsMouseDown(MouseButton button) => GetButtonState(_currentMouse, button) == ButtonState.Pressed;

        public Point MousePosition => _currentMouse.Position;
        public Point MouseDelta => _currentMouse.Position - _prevMouse.Position;
        public int ScrollDelta => _currentMouse.ScrollWheelValue - _prevMouse.ScrollWheelValue;

        private static ButtonState GetButtonState(MouseState mouse, MouseButton button) {
            return button switch {
                MouseButton.Left => mouse.LeftButton,
                MouseButton.Right => mouse.RightButton,
                MouseButton.Middle => mouse.MiddleButton,
                MouseButton.XButton1 => mouse.XButton1,
                MouseButton.XButton2 => mouse.XButton2,
                _ => ButtonState.Released
            };
        }
    }

    public enum MouseButton {
        Left, Right, Middle, XButton1, XButton2
    }
}