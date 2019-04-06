using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Silesian_Undergrounds.Engine.Common;

namespace Silesian_Undergrounds.Engine.UI.Controls
{
    public class Button : UIElement
    {
        private Texture2D normalTexture;
        private Texture2D hoverTexture;
        private SpriteFont font;
        private bool isHovering;

        private Func<bool> OnMouseClick;
        private Func<bool> OnMouseHoverStart;
        private Func<bool> OnMouseHoverStop;

        public Button(string text, float x, float y, float w, float h, Texture2D texture, string hoverTextureName, string font, UIElement parent) : 
            base(x, y, w, h, texture, parent)
        {
            isHovering = false;
            if (hoverTextureName != "")
                hoverTexture = TextureMgr.Instance.GetTexture(hoverTextureName);

            //font

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

        // draws text to fit specific boundires (Rectangle)
        /*private void DrawString(SpriteBatch spriteBatch)
        {
            Vector2 size = ButtonTextFont.MeasureString(Text);

            float xScale = (Rectangle.Width / size.X);
            float yScale = (Rectangle.Height / size.Y);

            float scale = Math.Min(xScale, yScale);


            int strWidth = (int)Math.Round(size.X * scale);
            int strHeight = (int)Math.Round(size.Y * scale);
            Vector2 position = new Vector2();
            position.X = (((Rectangle.Width - strWidth) / 2) + Rectangle.X);
            position.Y = (((Rectangle.Height - strHeight) / 2) + Rectangle.Y);


            float rotation = 0.0f;
            Vector2 spriteOrigin = new Vector2(0, 0);
            float spriteLayer = 0.0f;
            SpriteEffects spriteEffects = new SpriteEffects();


            spriteBatch.DrawString(ButtonTextFont, Text, position, Color.Black, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        }*/
    }
}
