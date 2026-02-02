using Basement.Core.Assets;
using Basement.Gameplay.Collision;
using Basement.Gameplay.World;
using Microsoft.Xna.Framework;

namespace Basement.Gameplay.Maps {
    public static class TileCollisionBuilder {
        public static void BuildFromObjectLayer(World.World world, TiledLayer collisionLayer) {
            if (collisionLayer.Type != "objectgroup" || collisionLayer.Objects is null) 
                return;

            foreach (TiledObject obj in collisionLayer.Objects) {
                AabbCollider collider = new AabbCollider(null, null, obj.Width, obj.Height);
                world.AddSolid(new StaticBody(new Vector2(obj.X, obj.Y), collider));
            }
        }
    }
}