using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Engine.UI.Controls
{
    public class Button : UIElement
    {
        private Texture2D normalTexture;
        private Texture2D hoverTexture;
        private SpriteFont font;

        private bool isHovering;
        public string Text;

        private Func<bool> OnMouseClick;
        private Func<bool> OnMouseHoverStart;
        private Func<bool> OnMouseHoverStop;

        public Button(string text, float x, float y, float w, float h, Texture2D texture, string hoverTextureName, string f, UIElement parent) : 
            base(x, y, w, h, texture, parent)
        {
            isHovering = false;
            Text = text;

            if (hoverTextureName != "")
                hoverTexture = TextureMgr.Instance.GetTexture(hoverTextureName);

            normalTexture = texture;
            font = FontMgr.Instance.GetFont(f);

            OnMouseClick = MouseClick;
            OnMouseHoverStart = MouseHoverStart;
            OnMouseHoverStop = MouseHoverStop;
        }

        private bool MouseClick()
        {
            return true;
        }

        private bool MouseHoverStart()
        {
            if (hoverTexture != null)
                Texture = hoverTexture;

            return true;
        }

        private bool MouseHoverStop()
        {
            if (hoverTexture != null)
                Texture = normalTexture;

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsMouseButtonClicked())
                OnMouseClick.Invoke();

            if (IsMouseHovering() && !isHovering)
            {
                isHovering = true;
                OnMouseHoverStart.Invoke();
            }
            else if (!IsMouseHovering() && isHovering)
            {
                isHovering = false;
                OnMouseHoverStop.Invoke();
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);

            Vector2 size = font.MeasureString(Text);

            float marginX = rectangle.Width - size.X;
            float marginY = rectangle.Height - size.Y;

            marginX /= 2;
            marginY /= 2;

            Vector2 position = new Vector2(rectangle.X + marginX, rectangle.Y + marginY);

            batch.DrawString(font, Text, position, Color.Black);
        }
    }
}
