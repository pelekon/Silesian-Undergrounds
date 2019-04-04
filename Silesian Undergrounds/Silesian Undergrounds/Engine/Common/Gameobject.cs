using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Silesian_Undergrounds.Engine.Common
{
    public class GameObject
    {
        public Texture2D texture { get; set; }
        public Vector2 position;
        public int layer;
        public float rotation { get; protected set; }
        public float speed { get; protected set; }
        public Vector2 size;
        public Vector2? scale;
        public Color color = Color.White;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            }
        }

        public GameObject(Texture2D texture, Vector2 position, Vector2 size, int layer = 1, Vector2? scale = null)
        {
            this.texture = texture;
            this.position = position;
            this.size = size;
            this.layer = layer;
            this.scale = scale;
        }

        // causes movement
        public void AddForce(float forceX, float forceY)
        {
            position.X += forceX * speed;
            position.Y += forceY * speed;
        }

        public Vector2 GetTileWhereStanding()
        {
            return new Vector2((float)System.Math.Round(this.position.X / size.X) * size.X, (float)System.Math.Round(this.position.Y / size.Y) * size.Y);
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: texture, destinationRectangle: Rectangle, scale: scale, color: color);
        }

        // TODO: Remove this and split collisions to 2 sparate components:
        // Collision Box and Collider
        public virtual void NotifyCollision(GameObject gameobject) { }
    }
}
