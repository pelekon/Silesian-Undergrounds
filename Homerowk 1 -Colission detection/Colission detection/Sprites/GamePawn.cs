using System;
using System.Collections.Generic;
using Colission_detection.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Colission_detection.Sprites
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

                if ((this.Velocity.X > 0 && this.IsTouchingLeft(sprite)) ||
                    (this.Velocity.X < 0 & this.IsTouchingRight(sprite)))
                {
                    this.Velocity.X = 0;
                    if (game != null) game.isBackgroundGreenColor = false;
                }
                    

                if ((this.Velocity.Y > 0 && this.IsTouchingTop(sprite)) ||
                    (this.Velocity.Y < 0 & this.IsTouchingBottom(sprite)))
                {
                    this.Velocity.Y = 0;
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
