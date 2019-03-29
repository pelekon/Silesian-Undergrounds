using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silesian_Undergrounds.Engine.Common
{
    public class Gameobject
    {
        public Texture2D texture;
        public Vector2 position;
        protected int layer;
        public float rotation { get; protected set; }
        public float speed { get; protected set; }
        public Vector2 size;

        public Gameobject(Texture2D texture, Vector2 position, Vector2 size, int layer)
        {
            this.texture = texture;
            this.position = position;
            this.size = size;
            this.layer = layer;
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            }
        }

        // causes movement
        public void AddForce(float forceX, float forceY)
        {
            position.X += forceX * speed;
            position.Y += forceY * speed;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: texture, destinationRectangle: Rectangle);
        }

        // TODO: Remove this and split collisions to 2 sparate components:
        // Collision Box and Collider
        public virtual void NotifyCollision(Gameobject gameobject) { }
    }
}
