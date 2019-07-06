using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleSampleV3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch HUDSpriteBatch;

        //int targetFPS = 60;

        public List<GameObject> gameObjects = new List<GameObject>();
        public Map gameMap = new Map();

        public GameHUD gameHUD = new GameHUD();

        //Editor editor;
        public TiledMap tiledMap = new TiledMap();

        private Effect homeworkShader;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //graphics.PreferredBackBufferHeight = 800;
            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.IsFullScreen = false;
            //graphics.ApplyChanges(); // usually optional, changes should apply automatically

            ResolutionManager.Init(ref graphics);
            ResolutionManager.SetVirtualResolution(1280, 800);
            ResolutionManager.SetResolution(1280, 800, false);


            //graphics.SynchronizeWithVerticalRetrace = false; //Vsync
            //IsFixedTimeStep = true;
            //TargetElapsedTime = System.TimeSpan.FromMilliseconds(1000.0f / targetFPS);


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


#if DEBUG
            //editor = new Editor(this);
            //editor.Show();
#endif
            Camera.Initialize(zoomLevel:0.5f);
            base.Initialize();
            Global.Initialize(this);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            HUDSpriteBatch = new SpriteBatch(GraphicsDevice);

#if DEBUG
            //editor.LoadTextures(Content);
#endif


            gameMap.Load(content:Content);
            gameHUD.Load(content:Content);
            LoadLevel("level1.jorge");
            tiledMap.Load(Content, @"Tilemaps/small_town.tmx");
            homeworkShader = Content.Load<Effect>("Shaders/AlphaChangeShader");
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
                Exit();

            Input.Update();

            UpdateGameObjects(gameObjects, map:tiledMap);
            //gameMap.Update(gameObjects);
            var playerObject = gameObjects[0];
            UpdateCamera(playerObject.position);

#if DEBUG
            //editor.Update(gameObjects, gameMap);
# endif


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            ResolutionManager.BeginDraw();

            var transformMatrix = Camera.GetTransformMatrix();

            // Sorting mode BackToFront - layerDepth 0 = front, 1.0f = back
            // Blend states - read http://www.shawnhargreaves.com/blog/premultiplied-alpha.html

            //GetTextureCenter()
            var playerObject = gameObjects[0];
            var posOffset = playerObject.GetTextureCenter();
            Vector2 centerPos = new Vector2(graphics.PreferredBackBufferWidth / 2 + 20, graphics.PreferredBackBufferHeight / 2 - 20);
            homeworkShader.Parameters["centerPos"].SetValue(centerPos);
            homeworkShader.Parameters["radiusOfAlphaArea"].SetValue(50.0f);


#if DEBUG
            //editor.Draw(spriteBatch);
#endif
            tiledMap.Draw(spriteBatch, homeworkShader, transformMatrix);
            //gameMap.DrawWalls(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, transformMatrix);

            DrawGameObjects(gameObjects);

            spriteBatch.End();

            gameHUD.Draw(HUDSpriteBatch);

            base.Draw(gameTime);
        }



        public void LoadLevel(string levelName)
        {
            Global.levelName = levelName;

            LevelData levelData = XmlHelper.Load(@"Content/Levels/" + levelName);

            gameMap.walls = levelData.walls;
            gameMap.decorations = levelData.decorations;
            gameObjects = levelData.gameObjects;

            gameMap.LoadMap(Content);

            LoadInitializeGameObjects(gameObjects);
        }

        // Should use multithreading ????
        // is ContentManager thread safe ?
        public void LoadInitializeGameObjects(List<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Initialize();
                gameObject.Load(content: Content);
            }


            //Parallel.ForEach(gameObjects, gameObject =>
            //{
            //    gameObject.Initialize();
            //    gameObject.Load(content: Content);
            //});
        }

        // Could use multithreading ????
        public void UpdateGameObjects(List<GameObject> gameObjects, TiledMap map)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Update(gameObjects, map);
            }

            //Parallel.ForEach(gameObjects, gameObject =>
            //{
            //    gameObject.Update(gameObjects);
            //});

        }

        // Cannot use multithreading in this form - spriteBatch is not thread safe
        // Can use multiple spritebatches, but is it better ???
        // Besides what about position in layer - which sprite should be draw over/under which one
        // This can be fixed by setting proper layerDepth parameters for each sprite
        public void DrawGameObjects(List<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch);
            }

            foreach (var decoration in gameMap.decorations)
            {
                decoration.Draw(spriteBatch);
            }
        }

        private void UpdateCamera(Vector2 followPosition)
        {
            Camera.Update(followPosition);
        }

    }
}
