using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

using Silesian_Undergrounds.Engine.UI;
using Silesian_Undergrounds.Engine.UI.Controls;
using Silesian_Undergrounds.Engine.Common;

namespace Silesian_Undergrounds.Views
{
    class MainMenuView : UIArea
    {
        protected override void Initialize()
        {
            base.Initialize();

            Texture2D background = TextureMgr.Instance.GetTexture("background");
            Texture2D buttonBg = TextureMgr.Instance.GetTexture("box_lit");
            //SpriteFont buttonFont = content.Load<SpriteFont>("File");

            Image bg = new Image(0, 0, 100, 100, background, this);
            Button startGame = new Button("New game", 40, 5, 10, 10, buttonBg, "box", "File", this);
            Button settings = new Button("Settings", 40, 15, 10, 10, buttonBg, "box", "File", this);
            Button exit = new Button("Quit game", 40, 50, 10, 10, buttonBg, "box", "File", this);

            AddElement(bg);
            AddElement(startGame);
            AddElement(settings);
            AddElement(exit);
        }
    }
}
