using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.UI;
using Silesian_Undergrounds.Engine.UI.Controls;
using Silesian_Undergrounds.Engine.Utils;
using Microsoft.Xna.Framework;

namespace Silesian_Undergrounds.Views {
    class PauseView : UIArea{

        private Button endGameButton;
        private Button resumeButton;
        private Label message;

        protected override void Initialize()
        {
            base.Initialize();

            Texture2D buttonBg = TextureMgr.Instance.GetTexture("box_lit");
            this.message = new Label("Pause", 50, 20, 0, 0, Color.WhiteSmoke, this);
            this.resumeButton = new Button("Resume", 43, 30, 15, 8, buttonBg, "box", "File", this);
            this.endGameButton = new Button("End game", 43, 40, 15, 8, buttonBg, "box", "File", this);

            AddElement(this.endGameButton);
            AddElement(this.resumeButton);
            AddElement(this.message);
        }

        public Button GetEndGameButton()
        {
            return this.endGameButton;
        }

        public Button GetResumeButton()
        {
            return this.resumeButton;
        }
    }
}
