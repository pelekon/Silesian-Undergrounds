using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Engine.HUD;

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
        Scene scene;

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
            // TODO: Add your initialization logic here
            Window.AllowAltF4 = true;
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            ResolutionMgr.GameWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            ResolutionMgr.GameHeight = GraphicsDevice.DisplayMode.Height;
            //graphics.ToggleFullScreen();
            graphics.ApplyChanges();
            TextureMgr.Instance.SetCurrentContentMgr(Content);

            sceneMgr = new SceneManager();
            scene = SceneManager.LoadScene("drop", 64);

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

            // TODO: Add your update logic here
            scene.Update(gameTime);
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
            spriteBatch.Begin(transformMatrix: scene.camera.Transform);            
            scene.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            gameHUD.Draw(HUDspriteBatch);
            base.Draw(gameTime);
        }
    }
}
