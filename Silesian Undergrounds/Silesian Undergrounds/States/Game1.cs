using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Player;
using Silesian_Undergrounds.States.Controls;
using System.Runtime.CompilerServices;
using System;
using System.Diagnostics;

namespace Silesian_Undergrounds
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SceneManager sceneMgr;
        Scene scene;
        // player object
        Player player;
        Button buttonStartGame;
        Button buttonOptions;
        Button buttonQuit;

        public enum GameState { InGame, InMenu, InMenuSettings };
        private GameState CurrentState = GameState.InMenu;

        //TODO: make some sort of GameStateManager ;>

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
            // temporary?
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //graphics.ToggleFullScreen();
            graphics.ApplyChanges();
            TextureMgr.Instance.SetCurrentContentMgr(Content);

            // Game state initialization
            sceneMgr = new SceneManager();
            scene = SceneManager.LoadScene("warstwy", 64, player);
            //Instantiates our player at the position X = 100, Y = 100;, scale - the vector resizing the texture (here 2.times)
            player = new Player(new Vector2(100, 100), new Vector2(255, 255), 1, new Vector2(2f, 2f));
            scene = new Scene(player);

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

            //Loads our player's content
            player.LoadContent(Content);




            // menu state initialization
            Texture2D ButtonTextureClicked = Content.Load<Texture2D>("box_lit");
            Texture2D ButtonTextureNotClicked = Content.Load<Texture2D>("box");
            SpriteFont buttonFont = Content.Load<SpriteFont>("File");

            int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int padding = 100;

            this.buttonStartGame = new Button("New game", ButtonTextureNotClicked, ButtonTextureClicked, new Vector2((width - ButtonTextureClicked.Width)/2, 0 + padding), new Vector2(ButtonTextureClicked.Width, ButtonTextureClicked.Height), buttonFont);
            this.buttonOptions = new Button("Settings", ButtonTextureNotClicked, ButtonTextureClicked, new Vector2((width - ButtonTextureClicked.Width) / 2, (0 + 2*padding + ButtonTextureClicked.Height)), new Vector2(ButtonTextureClicked.Width, ButtonTextureClicked.Height), buttonFont);
            this.buttonQuit = new Button("Quit", ButtonTextureNotClicked, ButtonTextureClicked, new Vector2((width - ButtonTextureClicked.Width) / 2, 0 + 3 * padding + 2 * ButtonTextureClicked.Height), new Vector2(ButtonTextureClicked.Width, ButtonTextureClicked.Height), buttonFont);

            Func<Game1, Boolean> callbackQuitGame = g =>
            {
                Debug.WriteLine("Quiting the game!");
                g.Exit();
                return true;
            };

            Func<Game1, Boolean> callbackStartGame = g =>
            {
                Debug.WriteLine("Changing the game state!");
                g.changeGameState(GameState.InGame);
                return true;
            };

            this.buttonQuit.SetGame(this);
            this.buttonQuit.SetOnClickCallback(callbackQuitGame);

            this.buttonStartGame.SetGame(this);
            this.buttonStartGame.SetOnClickCallback(callbackStartGame);


            // TODO: use this.Content to load your game content here
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
                player.Update(gameTime);
            } 
            else if(CurrentState == GameState.InMenu)
            {
               buttonOptions.Update(gameTime);
               buttonStartGame.Update(gameTime);
               buttonQuit.Update(gameTime);
            }

            // temporary
           // buttonStartGame.Update(gameTime);


            player.Collision(scene.Gameobjects);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(); 
            if(CurrentState == GameState.InGame)
            {
                scene.Draw(gameTime, spriteBatch);
            } else if(CurrentState == GameState.InMenu)
            {
                buttonQuit.Draw(spriteBatch);
                buttonOptions.Draw(spriteBatch);
                buttonStartGame.Draw(spriteBatch);
            }
            
           
           // buttonStartGame.Draw(spriteBatch);
            spriteBatch.End();
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
