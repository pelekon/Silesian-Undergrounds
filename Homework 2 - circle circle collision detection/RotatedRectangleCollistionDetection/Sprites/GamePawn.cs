using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RotatedRectangleCollistionDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatedRectangleCollistionDetection.Sprites
{
    public class GamePawn : Sprite
    {
        public GamePawn(Texture2D texture, Input input, float speed)
          : base(texture, input, speed)
        {

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, Game1 game = null)
        {
            Move();
            if (game != null) game.isBackgroundGreenColor = true;
            foreach (var sprite in sprites)
            {
                if (sprite == this)
                    continue;

                if(this.BruteForcePerPixelCollision(sprite))
                {
                    if (game != null) game.isBackgroundGreenColor = false;
                }
            }
            
            Position += Velocity;
           

            Velocity = Vector2.Zero;
        }

        private void Move()
        {
            if (Keyboard.GetState().IsKeyDown(_input.Left))
                Velocity.X = -_speed;
            else if (Keyboard.GetState().IsKeyDown(_input.Right))
                Velocity.X = _speed;

            if (Keyboard.GetState().IsKeyDown(_input.Up))
                Velocity.Y = -_speed;
            else if (Keyboard.GetState().IsKeyDown(_input.Down))
                Velocity.Y = _speed;
        }
    }
}
