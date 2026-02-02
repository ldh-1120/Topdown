using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Basement.Graphics.Rendering;

public static class SpriteDraw {
#nullable enable
    public static void WithPixelPass(SpriteBatch spriteBatch, Action<SpriteBatch> draw, SpriteSortMode sortMode = SpriteSortMode.Deferred, Effect? effect = null, BlendState? blendState = null, SamplerState? samplerState = null, DepthStencilState? depthStencilState = null, RasterizerState? rasterizerState = null, Matrix? transformMatrix = null) {
        if (spriteBatch == null)
            throw new ArgumentNullException(nameof(spriteBatch));
        if (draw == null)
            throw new ArgumentNullException(nameof(draw));

        blendState ??= BlendState.AlphaBlend;
        samplerState ??= SamplerState.PointClamp;
        depthStencilState ??= DepthStencilState.None;
        rasterizerState ??= RasterizerState.CullNone;

        spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        draw(spriteBatch);
        spriteBatch.End();
    }

#nullable enable
    public static void WithEffectPass(SpriteBatch spriteBatch, Effect effect, Action<SpriteBatch> draw, BlendState? blendState = null, SamplerState? samplerState = null, DepthStencilState? depthStencilState = null, RasterizerState? rasterizerState = null, Matrix? transformMatrix = null, SpriteSortMode sortMode = SpriteSortMode.Immediate) {
        if (spriteBatch == null)
            throw new ArgumentNullException(nameof(spriteBatch));
        if (effect == null)
            throw new ArgumentNullException(nameof(effect));
        if (draw == null)
            throw new ArgumentNullException(nameof(draw));

        blendState ??= BlendState.AlphaBlend;
        samplerState ??= SamplerState.PointClamp;
        depthStencilState ??= DepthStencilState.None;
        rasterizerState ??= RasterizerState.CullNone;

        spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        draw(spriteBatch);
        spriteBatch.End();
    }

    public static Vector2 OriginCenter(Texture2D texture) {
        if (texture == null)
            throw new ArgumentNullException(nameof(texture));
        return new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
    }

    public static Vector2 OriginCenter(Rectangle rectangle) {
        return new Vector2(rectangle.Width * 0.5f, rectangle.Height * 0.5f);
    }

#nullable enable
    public static void DrawCentered(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, float rotation = 0f, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0f) {
        if (spriteBatch == null)
            throw new ArgumentNullException(nameof(spriteBatch));
        if (texture == null)
            throw new ArgumentNullException(nameof(texture));

        Vector2 origin = OriginCenter(texture);
        Vector2 actualScale = scale ?? Vector2.One;

        spriteBatch.Draw(texture, position, null, color, rotation, origin, actualScale, spriteEffects, layerDepth);
    }

#nullable enable
    public static void DrawCentered(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle, Vector2 position, Color color, float rotation = 0f, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0f) {
        if (spriteBatch == null)
            throw new ArgumentNullException(nameof(spriteBatch));
        if (texture == null)
            throw new ArgumentNullException(nameof(texture));

        Vector2 origin = OriginCenter(sourceRectangle);
        Vector2 actualScale = scale ?? Vector2.One;

        spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, actualScale, spriteEffects, layerDepth);
    }

#nullable enable
    public static void DrawStretched(SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, Color color, Rectangle? sourceRectangle = null, float rotation = 0f, Vector2? origin = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0f) {
        if (spriteBatch == null)
            throw new ArgumentNullException(nameof(spriteBatch));
        if (texture == null)
            throw new ArgumentNullException(nameof(texture));

        Vector2 actualOrigin = origin ?? Vector2.Zero;
        spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, actualOrigin, spriteEffects, layerDepth);
    }

    public static Vector2 PixelSnap(Vector2 vector) {
        return new Vector2(MathF.Round(vector.X), MathF.Round(vector.Y));
    }

    public static Vector2 PixelFloor(Vector2 vector) {
        return new Vector2(MathF.Floor(vector.X), MathF.Floor(vector.Y));
    }

    public static float LayerDepthFromY(float y, float worldMaxY, bool invert = false) {
        if (worldMaxY <= 0f)
            return 0f;

        float depth = MathHelper.Clamp(y / worldMaxY, 0f, 1f);
        return invert ? 1f - depth : depth;
    }
}