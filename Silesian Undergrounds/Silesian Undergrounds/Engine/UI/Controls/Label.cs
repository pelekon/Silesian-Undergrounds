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
    internal enum TextAlign
    {
        ALIGN_LEFT,
        ALIGN_CENTER,
        ALIGN_RIGHT,
    }

    class Label : UIElement
    {
        public string Text;
        private SpriteFont _font;
        private Color _color = Color.White;
        private TextAlign _textAlign = TextAlign.ALIGN_CENTER;

        public Label(string text, float x, float y, float w, float h, UIElement parent) : base (x, y, w, h, null, parent)
        {
            Text = text;
            _font = FontMgr.Instance.GetFont("Fonts/font");
            Texture = TextureMgr.Instance.GetTexture("default");
        }

        public Label(string text, float x, float y, float w, float h, string font, UIElement parent) : base(x, y, w, h, null, parent)
        {
            Text = text;
            _font = FontMgr.Instance.GetFont(font);
            Texture = TextureMgr.Instance.GetTexture("default");
        }

        public Label(string text, float x, float y, float w, float h, Color fontColor, UIElement parent) : base(x, y, w, h, null, parent)
        {
            Text = text;
            _font = FontMgr.Instance.GetFont("Fonts/font");
            _color = fontColor;
            Texture = TextureMgr.Instance.GetTexture("default");
        }

        public Label(string text, float x, float y, float w, float h, string texture, string font, UIElement parent) : base(x, y, w, h, null, parent)
        {
            Text = text;
            _font = FontMgr.Instance.GetFont(font);
            Texture = TextureMgr.Instance.GetTexture(texture);
        }

        public Label(string text, float x, float y, float w, float h, string font, TextAlign align, UIElement parent) : base(x, y, w, h, null, parent)
        {
            Text = text;
            _font = FontMgr.Instance.GetFont(font);
            Texture = TextureMgr.Instance.GetTexture("default");
            _textAlign = align;
        }

        public void SetTextAlign(TextAlign align)
        {
            _textAlign = align;
        }

        public void SetBackground(Texture2D bg)
        {
            Texture = bg;
        }

        public override void Draw(SpriteBatch batch)
        {
            if (Texture != null)
                base.Draw(batch);

            Vector2 size = _font.MeasureString(Text);

            float marginX = rectangle.Width - size.X;
            float marginY = rectangle.Height - size.Y;

            switch(_textAlign)
            {
                case TextAlign.ALIGN_LEFT:
                    marginX = 2;
                    marginY /= 2;
                    break;
                case TextAlign.ALIGN_CENTER:
                    marginX /= 2;
                    marginY /= 2;
                    break;
                case TextAlign.ALIGN_RIGHT:
                    int helper = rectangle.Width - 2;
                    marginX = helper - size.X;
                    marginY /= 2;
                    break;
            }

            Vector2 position = new Vector2(rectangle.X + marginX, rectangle.Y + marginY);

            batch.DrawString(_font, Text, position, _color);
        }
    }
}
