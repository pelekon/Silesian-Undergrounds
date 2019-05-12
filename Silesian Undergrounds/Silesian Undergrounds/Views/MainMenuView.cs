using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.UI;
using Silesian_Undergrounds.Engine.UI.Controls;
using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Views
{
    class MainMenuView : UIArea
    {

        private Button startGameButton;
        private Button settingButton;
        private Button exitButton;

        protected override void Initialize()
        {
            base.Initialize();

            Texture2D background = TextureMgr.Instance.GetTexture("background");
            Texture2D buttonBg = TextureMgr.Instance.GetTexture("box_lit");

            Image bg = new Image(0, 0, 100, 100, background, this);
            this.startGameButton = new Button("New game", 43, 10, 15, 8, buttonBg, "box", "File", this);
            this.settingButton = new Button("Settings", 43, 22, 15, 8, buttonBg, "box", "File", this);
            this.exitButton = new Button("Quit game", 43, 50, 15, 8, buttonBg, "box", "File", this);

            AddElement(bg);
            AddElement(this.startGameButton);
            AddElement(this.settingButton);
            AddElement(this.exitButton);
        }

        public Button GetStartGameButton() {
            return this.startGameButton;
        }
    }
}
