using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silesian_Undergrounds.Engine.Common
{
    class Gameobject
    {
        Texture2D texure;
        Vector2 position;
        float rotation;
        float speed;

        void AddForce(float forceX, float forceY)
        {
            position.X += forceX;
            position.Y += forceY;
        }
    }
}
