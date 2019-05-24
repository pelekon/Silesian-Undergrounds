using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.UI;
using Silesian_Undergrounds.Engine.UI.Controls;
using Silesian_Undergrounds.Engine.Utils;
using Microsoft.Xna.Framework;

namespace Silesian_Undergrounds.Views {
    class LoadingView : UIArea{

        private Label story;

        protected override void Initialize()
        {
            base.Initialize();

            Texture2D buttonBg = TextureMgr.Instance.GetTexture("box_lit");

            this.story = new Label("Loading level ... ", 50, 50, 0, 0, Color.WhiteSmoke, this);


            AddElement(this.story);
        }
    }
}
