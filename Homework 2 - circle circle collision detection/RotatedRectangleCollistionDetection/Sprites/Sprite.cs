using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RotatedRectangleCollistionDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;

namespace RotatedRectangleCollistionDetection.Sprites
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

        // draws circle using monogame extension library
        public virtual void DrawWithRotation(SpriteBatch spriteBatch)
        {
            var origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
            //spriteBatch.Draw(_texture, null, null, null, rotationOrigin, 0.5f, null, TextureColor, SpriteEffects.None, 0f);
            spriteBatch.Draw(_texture, Rectangle, null, TextureColor, 0.5f, origin, SpriteEffects.None, 0f);
        }

       public virtual void DrawAsCircle(SpriteBatch spriteBatch)
       {
            spriteBatch.DrawCircle(Position, 30, 60, Color.Blue, 30);
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
        }



        public bool BruteForcePerPixelCollision(Sprite otherSprite)
        {
            var sourceColors = new Color[_texture.Width * _texture.Height];
            _texture.GetData(sourceColors);

            var targetColors = new Color[otherSprite._texture.Width * otherSprite._texture.Height];
            otherSprite._texture.GetData(targetColors);

            // two avoid intensive CPU calculation count the intersecting rectagle of two
            // shapes
            var leftVertex = Math.Max(Rectangle.Left, otherSprite.Rectangle.Left);
            var topVertex = Math.Max(Rectangle.Top, otherSprite.Rectangle.Top);
            var intersectingRectangleWidth = Math.Min(Rectangle.Right, otherSprite.Rectangle.Right) - leftVertex;
            var intersectingRectangleHeight = Math.Min(Rectangle.Bottom, otherSprite.Rectangle.Bottom) - topVertex;

            var intersectingRectangle = new Rectangle(leftVertex, topVertex, intersectingRectangleWidth, intersectingRectangleHeight);

            // loop through intersecting rectangle pixels
            // and if any is 'visible' (alfa > 0) then
            // we have intersection
            for (var x = intersectingRectangle.Left; x < intersectingRectangle.Right; x++)
            {
                for (var y = intersectingRectangle.Top; y < intersectingRectangle.Bottom; y++)
                {
                    var sourceColor = sourceColors[(x - Rectangle.Left) + (y - Rectangle.Top) * _texture.Width];
                    var targetColor = targetColors[(x - otherSprite.Rectangle.Left) + (y - otherSprite.Rectangle.Top) * otherSprite._texture.Width];

                    if (targetColor.A > 0 && sourceColor.A > 0) return true;
                }
            }
            return false;
        }
    }
}
