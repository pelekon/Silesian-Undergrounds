using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Engine.UI.Controls
{
    class Label : UIElement
    {
        public string Text;
        SpriteFont _font;
        Color _color = Color.White;

        public Label(string text, float x, float y, float w, float h, UIElement parent) : base (x, y, w, h, null, parent)
        {
            Text = text;
            _font = FontMgr.Instance.GetFont("Fonts/font");
        }

        public Label(string text, float x, float y, float w, float h, string font, UIElement parent) : base(x, y, w, h, null, parent)
        {
            Text = text;
            _font = FontMgr.Instance.GetFont("font");
        }

        public Label(string text, float x, float y, float w, float h, Color fontColor, UIElement parent) : base(x, y, w, h, null, parent)
        {
            Text = text;
            _font = FontMgr.Instance.GetFont("Fonts/font");
            _color = fontColor;
        }

        public override void Draw(SpriteBatch batch)
        {
            //base.Draw(batch);

            Vector2 size = _font.MeasureString(Text);

            float marginX = rectangle.Width - size.X;
            float marginY = rectangle.Height - size.Y;

            marginX /= 2;
            marginY /= 2;

            Vector2 position = new Vector2(rectangle.X + marginX, rectangle.Y + marginY);

            batch.DrawString(_font, Text, position, _color);
        }
    }
}
