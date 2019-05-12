using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Views;
using System;
using System.Collections.Generic;

namespace Silesian_Undergrounds
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public List<String> scenes = new List<String>();
        public int levelCounter = 0;
        public bool isPlayerInMaineMenu = true;

        Scene scene;

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

            scenes.Add("special_items");
            scenes.Add("drop");
            scenes.Add("drop2");
            scenes.Add("drop3");
            

            TextureMgr.Instance.SetCurrentContentMgr(Content);
            FontMgr.Instance.SetCurrentContentMgr(Content);

            //#if DEBUG
            //scene = LevelsManagement();
            //#else
            scene = new Scene(new MainMenuView());
            //#endif

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
            if(!scene.isPaused && isPlayerInMaineMenu)
            {
                scene = LevelsManagement();
                isPlayerInMaineMenu = false;
            }

            if (!scene.isEnd)
                scene.Update(gameTime);
            else
            {
                scene = LevelsManagement();
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

            scene.Draw();

            base.Draw(gameTime);
        }

        protected Scene LevelsManagement()
        {
            var sceneName = scenes[levelCounter];
            #if DEBUG
            System.Diagnostics.Debug.WriteLine("Current scene: " + sceneName);
            #endif
            levelCounter++;
            return SceneManager.LoadScene(sceneName, 64);
        }
    }
}
