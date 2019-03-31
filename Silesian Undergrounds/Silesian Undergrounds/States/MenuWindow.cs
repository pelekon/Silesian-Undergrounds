using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Silesian_Undergrounds.States
{
    class MenuWindow
    {

        private Texture2D ButtonTexture;
        private Texture2D BackgroundTexture;

        public MenuWindow(Game1 game, GraphicsDevice graphicDevice, Microsoft.Xna.Framework.Content.ContentManager content) 
        {
            ButtonTexture = content.Load<Texture2D>("asdasd");
            BackgroundTexture = content.Load<Texture2D>("asdasd");
        }

    }
}
