using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Engine.Enum;
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

            scenes.Add("level_1");
            scenes.Add("level_2");
            scenes.Add("level_3");
            scenes.Add("t");
            scenes.Add("drop");
            scenes.Add("drop2");
            //scenes.Add("drop3");

            TextureMgr.Instance.SetCurrentContentMgr(Content);
            FontMgr.Instance.SetCurrentContentMgr(Content);

            scene = SetMainMenuScene();

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
            Scene sceneToLoad;
            if (levelCounter == scenes.Count)
            {
                sceneToLoad = SceneManager.LoadScene(sceneName, 64);
                sceneToLoad.SetLastScene(true);
                sceneToLoad.SetOnWin(EndGamePlayerWin);
            }

            else
                sceneToLoad = SceneManager.LoadScene(sceneName, 64);

            sceneToLoad.player.SetOnDeath(EndGamePlayerDie);
            sceneToLoad.SetEndGameButtonInPauseMenu(ReturnToMenu);
            if(levelCounter > 1)
                sceneToLoad.DecreaseHungerDropInterval();

            return sceneToLoad;
        }

        protected bool StartGame()
        {
            scene = LevelsManagement();
            return true;
        }

        protected bool ExitGame()
        {
            this.Exit();
            return true;
        }

        protected bool EndGamePlayerDie()
        {
            this.scene = SetEndGameScene(EndGameEnum.Lost);
            return true;
        }

        protected bool EndGamePlayerWin()
        {
            this.scene = SetEndGameScene(EndGameEnum.Win);
            return true;
        }

        protected bool StartView()
        {
            this.scene = SetStartView();
            return true;
        }

        protected bool ReturnToMenu()
        {
            levelCounter = 0;
            SceneManager.ClearPlayerStatistics();
            this.scene = SetMainMenuScene();
            return true;
        }

        protected Scene SetMainMenuScene()
        {
            MainMenuView mainMenu = new MainMenuView();
            mainMenu.GetStartGameButton().SetOnClick(StartView);
            mainMenu.GetExitButton().SetOnClick(ExitGame);
            return new Scene(mainMenu);
        }

        protected Scene SetStartView()
        {
            StartView startView = new StartView();
            startView.GetNextButton().SetOnClick(StartGame);
            return new Scene(startView);
        }

        protected Scene SetEndGameScene(EndGameEnum endGameEnum)
        {

            if(endGameEnum == EndGameEnum.Lost)
            {
                PlayerDieView endGameWhenPlayerDie = new PlayerDieView();
                endGameWhenPlayerDie.GetReturnToMenuButton().SetOnClick(ReturnToMenu);
                return new Scene(endGameWhenPlayerDie);
            }
            else
            {
                PlayerWinView endGameWhenPlayerWin = new PlayerWinView();
                endGameWhenPlayerWin.GetReturnToMenuButton().SetOnClick(ReturnToMenu);
                return new Scene(endGameWhenPlayerWin);
            }

        }
    }
}
