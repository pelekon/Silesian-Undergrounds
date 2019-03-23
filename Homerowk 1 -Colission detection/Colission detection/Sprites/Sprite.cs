using Colission_detection.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


/**
 *  Abstract class implemeted by all sprites 
 * 
 */
namespace Colission_detection.Sprites
{
    public class Sprite
    {
        // for random color generation
        private Random rnd = new Random();
        // texture of the sprite
        private Texture2D _texture;
        public Vector2 Position;
        public Vector2 Velocity;
        public Color TextureColor = Color.White;
        protected float _speed;
        // keys used to move the object
        protected Input _input;

     
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public Sprite(Texture2D texture, Input input, float speed)
        {
            _texture = texture;
            _input = input;
            _speed = speed;
        }

        // when sprite is capable of moving we need method for 'frame' updating
        public virtual void Update(GameTime gameTime, List<Sprite> sprites, Game1 game = null)
        {
        }

        // draws the sprite using given spriteBatch
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, TextureColor);
        }

        // gets random color but not the same as current
        public void ChangeForRandomColor()
        {
            Color randomColor;
            do
            {
                randomColor = Microsoft.Xna.Framework.Color.FromNonPremultiplied(rnd.Next(256), rnd.Next(256), rnd.Next(256), 0);
            } while (randomColor == TextureColor);

            Color[] tcolot = new Color[_texture.Width * _texture.Height];
            if (_texture == null) return;
            _texture.SetData<Color>(tcolot);
            //_texture.Dispose();
            // TextureColor = randomColor;
        }

        // region with boolean functions 
        // responsible for intersection of sprite detection
        protected bool IsTouchingLeft(Sprite sprite)
        {
            return this.Rectangle.Right + this.Velocity.X > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Left &&
              this.Rectangle.Bottom > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(Sprite sprite)
        {
            return this.Rectangle.Left + this.Velocity.X < sprite.Rectangle.Right &&
              this.Rectangle.Right > sprite.Rectangle.Right &&
              this.Rectangle.Bottom > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(Sprite sprite)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Top &&
              this.Rectangle.Right > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsTouchingBottom(Sprite sprite)
        {
            return this.Rectangle.Top + this.Velocity.Y < sprite.Rectangle.Bottom &&
              this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
              this.Rectangle.Right > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Right;
        }
    }
}
