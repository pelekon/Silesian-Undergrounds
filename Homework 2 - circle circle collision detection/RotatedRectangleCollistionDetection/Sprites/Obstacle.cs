using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatedRectangleCollistionDetection.Sprites
{
    public class Obstacle : Sprite
    {
        public Obstacle(Texture2D texture)
          : base(texture, null, 0)
        {

        }

        // the object should not move
        override public void Update(GameTime gameTime, List<Sprite> sprites, Game1 game = null) { }
    }
}
