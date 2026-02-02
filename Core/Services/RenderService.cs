using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Basement.Core.Services {
    public class RenderService {
        public Texture2D Pixel { get; private set; }

        public void Load(GraphicsDevice graphicsDevice) {
            Pixel = new Texture2D(graphicsDevice, 1, 1);
            Pixel.SetData([Color.White]);
        }
    }
}