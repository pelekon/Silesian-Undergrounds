using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silesian_Undergrounds.Engine.Common
{
    public class Gameobject
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 size;

        public Gameobject(Texture2D texture, Vector2 position, Vector2 size)
        {
            this.texture = texture;
            this.position = position;
            this.size = size;
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: texture, destinationRectangle: Rectangle);
        }

        // TODO: Remove this and split collisions to 2 sparate components:
        // Collision Box and Collider
        public virtual void NotifyCollision(Gameobject gameobject) { }
    }
}
