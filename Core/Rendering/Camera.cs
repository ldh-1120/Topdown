using System;
using Microsoft.Xna.Framework;

namespace Basement.Core.Rendering {
    public sealed class Camera {
        private const float MIN_ZOOM = 0.05f;
        private const float MAX_ZOOM = 10f;

        public Vector2 Position { get; private set; } = Vector2.Zero;
        public float Rotation { get; private set; } = 0f;
        public float Zoom { get; private set; } = 1f;

        public Rectangle ViewportRect { get; private set; }
        public Rectangle? WorldBounds { get; private set; } = null;

        public bool SmoothFollowEnabled { get; set; } = true;
        public float SmoothHalfLifeSeconds { get; set; } = 0.08f;

        public bool ShakeEnabled => _shakeTimeRemaining > 0f;
        public float ShakeIntensityPixels { get; private set; } = 0f;

        private Vector2 _targetPosition;
        private Vector2 _shakeOffset;
        private float _shakeTimeRemaining;
        private float _shakeTotalTime;

        private uint _rngState = 0x12345678;

        public Camera(Rectangle viewportRect) {
            ViewportRect = viewportRect;
            _targetPosition = Position;
        }

        public void ResizeViewport(Rectangle viewportRect) {
            ViewportRect = viewportRect;
            Position = ClampToBounds(Position);
            _targetPosition = ClampToBounds(_targetPosition);
        }

        public void SetWorldBounds(Rectangle? bounds) {
            WorldBounds = bounds;
            Position = ClampToBounds(Position);
            _targetPosition = ClampToBounds(_targetPosition);
        }

        public void SetZoom(float zoom) {
            Zoom = MathHelper.Clamp(zoom, MIN_ZOOM, MAX_ZOOM);
            Position = ClampToBounds(Position);
            _targetPosition = ClampToBounds(_targetPosition);
        }

        public void SetRotation(float radians) => Rotation = radians;

        public void SnapTo(Vector2 worldCenter) {
            _targetPosition = ClampToBounds(worldCenter);
            Position = _targetPosition;
        }

        public void Follow(Vector2 worldCenter) => _targetPosition = ClampToBounds(worldCenter);

        public void AddShake(float intensityPixels, float durationSeconds) {
            if (intensityPixels <= 0f || durationSeconds <= 0f)
                return;

            ShakeIntensityPixels = MathF.Max(ShakeIntensityPixels, intensityPixels);
            if (_shakeTimeRemaining <= 0f) {
                _shakeTimeRemaining = durationSeconds;
                _shakeTotalTime = durationSeconds;
            } else {
                _shakeTimeRemaining = MathF.Max(_shakeTimeRemaining, durationSeconds);
                _shakeTotalTime = MathF.Max(_shakeTotalTime, durationSeconds);
            }
        }

        public void Update(GameTime gameTime) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (SmoothFollowEnabled)
                Position = Damp(Position, _targetPosition, SmoothHalfLifeSeconds, deltaTime);
            else
                Position = _targetPosition;

            Position = ClampToBounds(Position);

            _shakeOffset = Vector2.Zero;
            if (_shakeTimeRemaining > 0f) {
                _shakeTimeRemaining -= deltaTime;
                float time = MathF.Max(_shakeTimeRemaining, 0f);
                float fade = _shakeTotalTime <= 0f ? 0f : time / _shakeTotalTime;

                float rx = NextFloatMinus1To1();
                float ry = NextFloatMinus1To1();
                _shakeOffset = new Vector2(rx, ry) * ShakeIntensityPixels * fade;
                if (_shakeTimeRemaining <= 0f) {
                    _shakeTimeRemaining = 0f;
                    _shakeTotalTime = 0f;
                    ShakeIntensityPixels = 0f;
                }
            }
        }

        public Matrix GetViewMatrix() {
            Vector2 viewportCenter = new Vector2(ViewportRect.Width * 0.5f, ViewportRect.Height * 0.5f);
            Vector2 finalPos = Position + _shakeOffset;

            return Matrix.CreateTranslation(new Vector3(-finalPos, 0f)) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Zoom, Zoom, 1f) * Matrix.CreateTranslation(new Vector3(viewportCenter, 0f));
        }

        public Vector2 WorldToScreen(Vector2 worldPos) {
            Vector2 viewportCenter = new Vector2(ViewportRect.Width * 0.5f, ViewportRect.Height * 0.5f);
            Vector2 finalPos = Position + _shakeOffset;

            Vector2 vector = worldPos - finalPos;
            if (Rotation != 0f) {
                float cos = MathF.Cos(Rotation);
                float sin = MathF.Sin(Rotation);
                vector = new Vector2(vector.X * cos - vector.Y * sin, vector.X * sin + vector.Y * cos);
            }
            vector *= Zoom;
            vector += viewportCenter;

            return vector;
        }

        public Vector2 ScreenToWorld(Vector2 screenPos) {
            Vector2 viewportCenter = new Vector2(ViewportRect.Width * 0.5f, ViewportRect.Height * 0.5f);
            Vector2 vector = screenPos - viewportCenter;

            vector /= Zoom;
            if (Rotation != 0f) {
                float cos = MathF.Cos(-Rotation);
                float sin = MathF.Sin(-Rotation);
                vector = new Vector2(vector.X * cos - vector.Y * sin, vector.X * sin + vector.Y * cos);
            }

            Vector2 finalPos = Position + _shakeOffset;
            return vector + finalPos;
        }

        private Vector2 ClampToBounds(Vector2 desiredCenter) {
            if (WorldBounds is null)
                return desiredCenter;

            Rectangle bounds = WorldBounds.Value;
            float halfWidth = ViewportRect.Width * 0.5f / Zoom;
            float halfHeight = ViewportRect.Height * 0.5f / Zoom;

            float minX = bounds.Left + halfWidth;
            float maxX = bounds.Right - halfWidth;
            float minY = bounds.Top + halfHeight;
            float maxY = bounds.Bottom - halfHeight;

            float clampedX;
            if (minX > maxX)
                clampedX = (bounds.Left + bounds.Right) * 0.5f;
            else
                clampedX = MathHelper.Clamp(desiredCenter.X, minX, maxX);

            float clampedY;
            if (minY > maxY)
                clampedY = (bounds.Top + bounds.Bottom) * 0.5f;
            else
                clampedY = MathHelper.Clamp(desiredCenter.Y, minY, maxY);

            return new Vector2(clampedX, clampedY);
        }

        private static Vector2 Damp(Vector2 current, Vector2 target, float halfLife, float deltaTime) {
            if (halfLife <= 0f)
                return target;

            float lambda = MathF.Pow(0.5f, deltaTime / halfLife);
            return target + (current - target) * lambda;
        }

        private float NextFloatMinus1To1() {
            uint x = _rngState;
            x ^= x << 13;
            x ^= x >> 17;
            x ^= x << 5;
            _rngState = x;

            float u = (x & 0x00FFFFFF) / (float)0x01000000;
            return u * 2f - 1f;
        }
    }
}