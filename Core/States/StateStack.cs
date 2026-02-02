using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Basement.States {
    public class StateStack {
        private readonly List<IGameState> _stack = new();
        private readonly Queue<Action> _pending = new();

        public int Count => _stack.Count;
#nullable enable
        public IGameState? Top => _stack.Count > 0 ? _stack[^1] : null;

        public void Push(IGameState state) {
            _pending.Enqueue(() => {
                if (_stack.Count > 0)
                    _stack[^1].OnPause();

                _stack.Add(state);
                state.OnEnter();
            });
        }

        public void Pop() {
            _pending.Enqueue(() => {
                if (_stack.Count == 0)
                    return;

                IGameState top = _stack[^1];
                top.OnExit();
                _stack.RemoveAt(_stack.Count - 1);
                if (_stack.Count > 0)
                    _stack[^1].OnResume();
            });
        }

        public void Replace(IGameState state) {
            _pending.Enqueue(() => {
                if (_stack.Count > 0) {
                    IGameState top = _stack[^1];
                    top.OnExit();
                    _stack.RemoveAt(_stack.Count - 1);
                }

                if (_stack.Count > 0)
                    _stack[^1].OnPause();
                _stack.Add(state);
                state.OnEnter();
            });
        }

        public void Clear() {
            _pending.Enqueue(() => {
                for (int i = _stack.Count - 1; i >= 0; i--)
                    _stack[i].OnExit();
                _stack.Clear();
            });
        }

        public void Update(GameTime gameTime) {
            if (_stack.Count == 0) {
                FlushPending();
                return;
            }
            
            int startIndex = _stack.Count - 1;
            for (int i = _stack.Count - 1; i >= 0; i--) {
                startIndex = i;
                if (!_stack[i].AllowUpdateBelow)
                    break;
            }

            for (int i = startIndex; i < _stack.Count; i++)
                _stack[i].Update(gameTime);

            FlushPending();
        }

        public void HandleInput(GameTime gameTime) {
            for (int i = _stack.Count - 1; i >= 0; i--) {
                if (_stack[i].HandleInput(gameTime))
                    break;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if (_stack.Count == 0)
                return;

            int startIndex = _stack.Count - 1;
            for (int i = _stack.Count - 1; i >= 0; i--) {
                startIndex = i;
                if (!_stack[i].AllowDrawBelow)
                    break;
            }

            for (int i = startIndex; i < _stack.Count; i++)
                _stack[i].Draw(gameTime, spriteBatch);
        }

        private void FlushPending() {
            while (_pending.Count > 0)
                _pending.Dequeue().Invoke();
        }
    }
}