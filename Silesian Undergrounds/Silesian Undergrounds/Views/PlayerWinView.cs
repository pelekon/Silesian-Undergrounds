using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.UI;
using Silesian_Undergrounds.Engine.UI.Controls;
using Silesian_Undergrounds.Engine.Utils;
using Microsoft.Xna.Framework;

namespace Silesian_Undergrounds.Views {
    class PlayerWinView : UIArea
    {

        private Button returnToMenuButton;
        private Label message;

        protected override void Initialize()
        {
            base.Initialize();

            Texture2D background = TextureMgr.Instance.GetTexture("background_2");
            base.AddBackground(new Image(0, 0, 100, 100, background, this));

            Texture2D buttonBg = TextureMgr.Instance.GetTexture("box_lit");
            this.message = new Label("You win!", 50, 20, 0, 0, Color.Green, this);

            this.returnToMenuButton = new Button("Back to menu", 43, 30, 15, 8, buttonBg, "box", "File", this);

            AddElement(returnToMenuButton);
            AddElement(message);
        }

        public Button GetReturnToMenuButton()
        {
            return this.returnToMenuButton;
        }
    }
}
