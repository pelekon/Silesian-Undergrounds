using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PdOne.Misc
{
    public class DrawableObject
    {
        private Texture2D texture;

        private float posX;
        private float posY;
        private float speed = 1.5f;
        float scale;

        public DrawableObject(Texture2D t, float x, float y, float s)
        {
            texture = t;
            posX = x;
            posY = y;
            scale = s;
        }

        public Vector2 GetPosition() { return new Vector2(posX, posY); }
        public Vector2 GetScaleVector() { return new Vector2(scale, scale); }
        public Texture2D GetTexture() { return texture; }

        public bool CheckForColision(DrawableObject obj)
        {
            bool areColliding = false;

            if (posX < (obj.posX + (obj.texture.Width * obj.scale)) && (posX + (texture.Width * scale)) > obj.posX &&
                posY < (obj.posY + (obj.texture.Height * obj.scale)) && (posY + (texture.Height * scale)) > obj.posY)
                areColliding = true;

            if (!areColliding)
            {
                if (obj.posX < (posX + (texture.Width * scale)) && (obj.posX + (obj.texture.Width * obj.scale)) > posX &&
                obj.posY < (posY + (texture.Height * scale)) && (obj.posY + (obj.texture.Height * obj.scale)) > posY)
                    areColliding = true;
            }


            return areColliding;
        }

        public void AddForce(float forceX, float forceY)
        {
            posX += forceX * speed;
            posY += forceY * speed;
        }
    }
}
