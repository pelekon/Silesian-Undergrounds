using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Silesian_Undergrounds.Engine.Common;

namespace Silesian_Undergrounds.Engine.Scene {
    class Tile : Gameobject{

        private Texture2D texture2D;
        private Vector3 position; // X & Y + Z as layer depth
        private Vector2 size;

        public Tile(Texture2D texture2D, Vector3 position, Vector2 size):base(texture2D, position, size)
        {
            this.texture2D = texture2D;
            this.position = position;
            this.size = size;
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
            spriteBatch.Draw(texture: texture2D, destinationRectangle: Rectangle, layerDepth: position.Z);
        }

    }
}
