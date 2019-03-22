using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silesian_Undergrounds.Engine.Common
{
    public class Gameobject
    {
        public Texture2D texture;
        public Vector2 position;
        float rotation;
        float speed;
        public Vector2 size;

        public Gameobject(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            }
        }

        protected void AddForce(float forceX, float forceY)
        {
            position.X += forceX;
            position.Y += forceY;
        }

        public virtual void NotifyCollision(Gameobject gameobject) { }
    }
}
