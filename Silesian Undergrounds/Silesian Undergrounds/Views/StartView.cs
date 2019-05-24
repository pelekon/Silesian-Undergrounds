using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.UI;
using Silesian_Undergrounds.Engine.UI.Controls;
using Silesian_Undergrounds.Engine.Utils;
using Microsoft.Xna.Framework;
using System;

namespace Silesian_Undergrounds.Views {
    class StartView : UIArea{

        private Button readyButton;
        private Button controlsButton;
        private Label story;

        protected override void Initialize()
        {
            base.Initialize();

            Texture2D buttonBg = TextureMgr.Instance.GetTexture("box_lit");

            this.story = new Label("Hello, Miner!" + Environment.NewLine + Environment.NewLine +
                "As a result of a powerful earthquake, you fell into a huge chasm that opened under your feet." + Environment.NewLine +
                "You are now in an underground world full of traps and monstrous creatures." + Environment.NewLine + Environment.NewLine +
                "You can get out of here by climbing up the ladders on each floor." + Environment.NewLine +
                "You can also find metal ores to buy something in ancient shops." + Environment.NewLine + Environment.NewLine +
                "Magic bottles can enhance your skills." + Environment.NewLine +
                "And remember to eat. Otherwise, your vitality will start to drop. " + Environment.NewLine + Environment.NewLine +
                "Good luck!" + Environment.NewLine 
                , 50, 20, 0, 0, Color.WhiteSmoke, this);

            this.readyButton = new Button("I am ready!", 53, 60, 15, 8, buttonBg, "box", "File", this);
            this.controlsButton = new Button("Show me controls", 33, 60, 15, 8, buttonBg, "box", "File", this);

            AddElement(this.readyButton);
            AddElement(this.controlsButton);
            AddElement(this.story);
        }

        public Button GetReadyButton()
        {
            return this.readyButton;
        }

        public Button GetControlsButton()
        {
            return this.controlsButton;
        }
    }
}
