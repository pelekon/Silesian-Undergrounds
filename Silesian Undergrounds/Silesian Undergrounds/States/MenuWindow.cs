using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silesian_Undergrounds.States.Controls;

namespace Silesian_Undergrounds.States
{
    class MenuWindow
    {

        #region PROPERTIES

        private List<Button> MenuControls;

        #endregion

        #region CONSTRUCTOR

        public MenuWindow(Game1 game, GraphicsDevice graphicDevice, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Texture2D ButtonTextureClicked = content.Load<Texture2D>("box.png");
            Texture2D ButtonTextureNotClicked = content.Load<Texture2D>("box_lit.png");
            Texture2D BackgroundTexture = content.Load<Texture2D>("background.png");
        }

        #endregion

        #region FACTORY

      //  private Button BuildButton()
      //  {
            //return new Button();
      //  }
        #endregion

        #region BUTTON_EVENT_HANDLER

        private void OnMenuButtonClick(Button target)
        {
            switch(target.Text)
            {
                case "New Game":
                case "Options":
                case "Exit":
                    break;
            }
        }

        #endregion
    }
}
