using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Silesian_Undergrounds.Engine.UI
{
    public class UIElement
    {
        private Vector2 _position;
        public Vector2 Position => _position;

        public Texture2D Texture { get; private set; }
        public int Width;
        public int Height;

        public UIElement(float x, float y, Texture2D text)
        {
            _position = new Vector2();
            SetPosition(x, y);
            Texture = text;
        }

        public void SetPosition(float x, float y)
        {
            if (x > 100 || y > 100 || x < 0 || y < 0)
                throw new WrongElementPositionException("Wrong position for UIElement");

            _position.X = x;
            _position.Y = y;
        }

        protected bool IsMouseButtonClicked()
        {
            MouseState mouseState = Mouse.GetState();

            return IsMouseHovering() && mouseState.LeftButton == ButtonState.Pressed;
        }

        protected bool IsMouseHovering()
        {
            MouseState mouseState = Mouse.GetState();

            // get current Mouse bounds
            Rectangle mouseBounds = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
            //if (mouseBounds.Intersects(this.Rectangle)) return true;


            return false;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch batch, float xUnit, float yUnit)
        {
            batch.Draw(Texture, new Rectangle((int)(_position.X * xUnit), (int)(_position.Y * yUnit), Width, Height), Color.White);
        }
    }

    internal class WrongElementPositionException : Exception
    {
        internal WrongElementPositionException(string msg) : base (msg) { }
    }
}
