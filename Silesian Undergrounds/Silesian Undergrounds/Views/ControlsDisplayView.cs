using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.UI;
using Silesian_Undergrounds.Engine.UI.Controls;
using Silesian_Undergrounds.Engine.Utils;
using Microsoft.Xna.Framework;
using System;

namespace Silesian_Undergrounds.Views {
    class ControlsDisplayView : UIArea {

        private Button nextButton;
        private Label controls;

        protected override void Initialize()
        {
            base.Initialize();

            Texture2D buttonBg = TextureMgr.Instance.GetTexture("box_lit");

            Texture2D background = TextureMgr.Instance.GetTexture("background_2");
            base.AddBackground(new Image(0, 0, 100, 100, background, this));

            this.controls = new Label("Controls" + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                "W - move up " + Environment.NewLine + Environment.NewLine +
                "S - move down " + Environment.NewLine + Environment.NewLine +
                "D - move rigth " + Environment.NewLine + Environment.NewLine +
                "A - move left " + Environment.NewLine + Environment.NewLine +
                "SPACE - attack " + Environment.NewLine
                , 50, 30, 0, 0, Color.WhiteSmoke, this);

            this.nextButton = new Button("I am really ready now!", 43, 50, 15, 8, buttonBg, "box", "File", this);

            AddElement(this.nextButton);
            AddElement(this.controls);
        }

        public Button GetNextButton()
        {
            return this.nextButton;
        }

    }
}
