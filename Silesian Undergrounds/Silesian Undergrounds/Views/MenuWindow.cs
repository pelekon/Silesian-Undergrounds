using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.States.Controls;

namespace Silesian_Undergrounds.Views
{
    class MenuWindow
    {

        #region PROPERTIES

        private List<Button> MenuControls;
        private Game1 Game;
        private Texture2D ButtonTextureClicked;
        private Texture2D ButtonTextureNotClicked;
        private Game1 game;
        //private Texture2D BackgroundTexture;
        #endregion

        #region CONSTRUCTOR

        public MenuWindow(Game1 game)
        {
            this.MenuControls = new List<Button>();
            this.Game = game;
        }

        #endregion


        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.ButtonTextureClicked = content.Load<Texture2D>("box");
            this.ButtonTextureNotClicked = content.Load<Texture2D>("box_lit");
            //this.BackgroundTexture = content.Load<Texture2D>("background.png");
            SpriteFont buttonFont = content.Load<SpriteFont>("File");

            int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int padding = 100;

            Button btnQuit = new Button("Quit", ButtonTextureNotClicked, ButtonTextureClicked, new Vector2((width - ButtonTextureClicked.Width) / 2, 0 + 3 * padding + 2 * ButtonTextureClicked.Height), new Vector2(ButtonTextureClicked.Width, ButtonTextureClicked.Height), buttonFont);
            Button btnSettings = new Button("Settings", ButtonTextureNotClicked, ButtonTextureClicked, new Vector2((width - ButtonTextureClicked.Width) / 2, (0 + 2 * padding + ButtonTextureClicked.Height)), new Vector2(ButtonTextureClicked.Width, ButtonTextureClicked.Height), buttonFont);
            Button btnNewGame = new Button("New game", ButtonTextureNotClicked, ButtonTextureClicked, new Vector2((width - ButtonTextureClicked.Width) / 2, 0 + padding), new Vector2(ButtonTextureClicked.Width, ButtonTextureClicked.Height), buttonFont);

            MenuControls.Add(btnNewGame);
            MenuControls.Add(btnSettings);
            MenuControls.Add(btnQuit);

            Func<Game1, Boolean> callbackQuitGame = g =>
            {
                g.Exit();
                return true;
            };

            Func<Game1, Boolean> callbackStartGame = g =>
            {
                g.changeGameState(GameState.InGame);
                return true;
            };


            btnNewGame.SetGame(Game);
            btnQuit.SetGame(Game);

            btnQuit.SetOnClickCallback(callbackQuitGame);
            btnNewGame.SetOnClickCallback(callbackStartGame);
        }


        public void Update(GameTime gameTime)
        {
            foreach(Button btn in MenuControls)
                btn.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button btn in MenuControls)
                btn.Draw(spriteBatch);
        }

    }
}
