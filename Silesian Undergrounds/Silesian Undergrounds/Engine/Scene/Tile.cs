using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Scene {
    class Tile : GameObject
    {
        public Tile(Texture2D texture2D, Vector2 position, Vector2 size, int layer) : base(texture2D, position, size, layer)
        {
            texture = texture2D;
            this.position = position;
            this.size = size;
            this.layer = layer;

            if (layer == 1)
            {
                BoxCollider collider = new BoxCollider(this, size.X, size.Y, 0, 0, false);
                AddComponent(collider);
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int) position.Y, (int)size.X, (int)size.Y);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
//            spriteBatch.Draw(texture: texture, destinationRectangle: Rectangle, layerDepth: layer);
        }

    }
}
