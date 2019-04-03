using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.States.Controls;
using System.Runtime.CompilerServices;
using System;
using System.Diagnostics;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Engine.HUD;
using Silesian_Undergrounds.Engine.Enum;

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
        SceneManager sceneMgr;
        Views.MenuWindow menuWindow;
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
            // window options initialization

            // TODO: Add your initialization logic here
            // Window.AllowAltF4 = true;
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            ResolutionMgr.GameWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            ResolutionMgr.GameHeight = GraphicsDevice.DisplayMode.Height;
            //graphics.ToggleFullScreen();
            

            graphics.ApplyChanges();
            TextureMgr.Instance.SetCurrentContentMgr(Content);

            // Game state initialization
            sceneMgr = new SceneManager();
            scene = SceneManager.LoadScene("camera", 64);

            menuWindow = new Views.MenuWindow(this);

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


            HUDspriteBatch = new SpriteBatch(GraphicsDevice);
            gameHUD.Load(content: Content);

            menuWindow.LoadContent(Content);

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
                menuWindow.Update(gameTime);
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
           
            if(CurrentState == GameState.InGame)
            {
                spriteBatch.Begin(transformMatrix: scene.camera.Transform);
                scene.Draw(gameTime, spriteBatch);
                scene.Draw(gameTime, spriteBatch);
                spriteBatch.End();
                gameHUD.Draw(HUDspriteBatch);
            } else if(CurrentState == GameState.InMenu)
            {
                spriteBatch.Begin();
                menuWindow.Draw(spriteBatch);
                spriteBatch.End();
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
