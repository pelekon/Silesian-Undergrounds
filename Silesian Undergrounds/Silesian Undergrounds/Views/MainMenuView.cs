using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

using Silesian_Undergrounds.Engine.UI;
using Silesian_Undergrounds.Engine.UI.Controls;
using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Views
{
    class MainMenuView : UIArea
    {
        public MainMenuView()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();

            Texture2D background = TextureMgr.Instance.GetTexture("background");
            Texture2D buttonBg = TextureMgr.Instance.GetTexture("box_lit");

            Image bg = new Image(0, 0, 100, 100, background, this);
            Button startGame = new Button("New game", 43, 10, 15, 8, buttonBg, "box", "File", this);
            Button settings = new Button("Settings", 43, 22, 15, 8, buttonBg, "box", "File", this);
            Button exit = new Button("Quit game", 43, 50, 15, 8, buttonBg, "box", "File", this);

            AddElement(bg);
            AddElement(startGame);
            AddElement(settings);
            AddElement(exit);
        }
    }
}
