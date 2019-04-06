using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Common;
using System.Runtime.CompilerServices;
using System;
using System.Diagnostics;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Engine.HUD;
using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Views;

namespace Silesian_Undergrounds
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch HUDspriteBatch;
        MainMenuView mainMenu;

        Scene scene;
        private GameState CurrentState = GameState.InMenu;
        public GameHUD gameHUD = new GameHUD(ResolutionMgr.TileSize);


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            #region GRAPHIC_SETTINGS_INIT
            // Window.AllowAltF4 = true;
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            ResolutionMgr.GameWidth = graphics.PreferredBackBufferWidth;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            ResolutionMgr.GameHeight = graphics.PreferredBackBufferHeight;
            //graphics.ToggleFullScreen();
            graphics.ApplyChanges();

            // Calculate inner unit value
            ResolutionMgr.yAxisUnit = ResolutionMgr.GameHeight / 100.0f;
            ResolutionMgr.xAxisUnit = ResolutionMgr.GameWidth / 100.0f;
            #endregion


            TextureMgr.Instance.SetCurrentContentMgr(Content);
            FontMgr.Instance.SetCurrentContentMgr(Content);

            scene = SceneManager.LoadScene("drop", 64);
            mainMenu = new MainMenuView();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Drawer.Initialize(spriteBatch, Content);

            HUDspriteBatch = new SpriteBatch(GraphicsDevice);
            gameHUD.Load(content: Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                scene.OpenPauseMenu();

           if (scene.isPaused)
                return;

            if (CurrentState == GameState.InGame)
            {
                GraphicsDevice.Clear(Color.AliceBlue);

                // TODO: Add your update logic here
                scene.Update(gameTime);
                // update our player sprite
            }
            else if(CurrentState == GameState.InMenu)
            {
                //menuWindow.Update(gameTime);
                mainMenu.Update(gameTime);
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Drawer.UpdateGameTime(gameTime);
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here

            switch (CurrentState)
            {
                case GameState.InGame:
                {

                    scene.Draw();
                    gameHUD.Draw(HUDspriteBatch);
                    break;
                }
                case GameState.InMenu:
                {
                    spriteBatch.Begin();
                    mainMenu.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                }
                case GameState.InMenuSettings:
                    break;
            }

            base.Draw(gameTime);
        }

        #region CUSTOM_METHODS

        public void changeGameState(GameState newState)
        {
            this.CurrentState = newState;
        }
        #endregion
    }
}
