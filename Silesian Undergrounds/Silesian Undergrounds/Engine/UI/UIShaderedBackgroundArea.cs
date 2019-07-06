using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silesian_Undergrounds.Engine.UI
{
    public class UIShaderedBackgroundArea: UIArea
    {
        public override void Draw(SpriteBatch batch)
        {
          foreach (var el in _elements)
          {
            // draw other elements not to be affected
            // by fogg shader
            if (el is Image)
                continue;

            el.Draw(batch);
          }
        }
    }
}
