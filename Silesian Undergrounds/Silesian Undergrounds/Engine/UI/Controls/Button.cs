using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Silesian_Undergrounds.Engine.UI.Controls
{
    public class Button : UIElement
    {
        private Texture2D hoverTexture;
        private bool isHovering;

        private Func<bool> OnMouseClick;
        private Func<bool> OnMouseHoverStart;
        private Func<bool> OnMouseHoverStop;

        public Button(string text, float x, float y, Texture2D texture, string hover) : 
            base(x, y, texture)
        {
            isHovering = false;
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
            return true;
        }

        private bool MouseHoverStop()
        {
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsMouseButtonClicked())
                OnMouseClick.Invoke();
        }


    }
}
