using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Engine.UI
{
    public class UIElement
    {
        private Vector2 _position;
        public Vector2 Position => _position;
        private Rectangle rectangle;

        public Texture2D Texture { get; protected set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        public UIElement Parent { get; private set; }

        public UIElement(float x, float y, float w, float h, Texture2D text, UIElement parent)
        {
            _position = new Vector2();
            SetPosition(x, y);
            Texture = text;
            Width = w;
            Height = h;

            Parent = parent;
            CalculateElementRectangle();
        }

        public void SetPosition(float x, float y)
        {
            if (x > 100 || y > 100 || x < 0 || y < 0)
                throw new WrongUIElementPositionException("Wrong position for UIElement");

            _position.X = x;
            _position.Y = y;
            CalculateElementRectangle();
        }

        public void SetHeight(float h)
        {
            Height = h;
            CalculateElementRectangle();
        }

        public void SetWidht(float w)
        {
            Width = w;
            CalculateElementRectangle();
        }

        private void CalculateElementRectangle()
        {
            rectangle = new Rectangle((int)(_position.X * ResolutionMgr.xAxisUnit), (int)(_position.Y * ResolutionMgr.yAxisUnit),
                (int)(Width * ResolutionMgr.xAxisUnit), (int)(Height * ResolutionMgr.yAxisUnit));
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
            if (mouseBounds.Intersects(rectangle))
                return true;


            return false;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, rectangle, Color.White);
        }
    }

    internal class WrongUIElementPositionException : Exception
    {
        internal WrongUIElementPositionException(string msg) : base (msg) { }
    }
}
