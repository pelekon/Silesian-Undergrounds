using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Engine.UI.Controls
{
    public class Image : UIElement
    {
        public Image(float x, float y, float w, float h, Texture2D texture, UIElement parent) : base(x, y, w, h, texture, parent) { }

        public Image(float x, float y, float w, float h, string textureName, UIElement parent) : base(x, y, w, h, null, parent)
        {
            Texture = TextureMgr.Instance.GetTexture(textureName);
        }
    }
}
