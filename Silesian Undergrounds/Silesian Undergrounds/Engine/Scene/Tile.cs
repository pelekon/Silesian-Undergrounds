using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Silesian_Undergrounds.Engine.Common;

namespace Silesian_Undergrounds.Engine.Scene {
    class Tile : Gameobject{

        private Texture2D texture2D;
        private Vector2 position;
        private int layer;
        private Vector2 size;

        public Tile(Texture2D texture2D, Vector2 position, Vector2 size, int layer):base(texture2D, position, size, layer)
        {
            this.texture2D = texture2D;
            this.position = position;
            this.size = size;
            this.layer = layer;
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
            spriteBatch.Draw(texture: texture2D, destinationRectangle: Rectangle, layerDepth: layer);
        }

    }
}
