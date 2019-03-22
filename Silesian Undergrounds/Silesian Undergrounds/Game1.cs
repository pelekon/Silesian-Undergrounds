using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

using Silesian_Undergrounds.Engine.Player;
using Silesian_Undergrounds.Engine.Common;

namespace Silesian_Undergrounds
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        List<Gameobject> gameobjects = new List<Gameobject>();
        List<Gameobject> toDelete = new List<Gameobject>();

        Texture2D pickableItemTexture;

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

            player = new Player(this.Content.Load<Texture2D>("character"), new Vector2(0, 0), new Vector2(50, 50));

            pickableItemTexture = Content.Load<Texture2D>("coal");
            SpawnPickableItems();
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

            DeleteScheduledObjects();

            player.Update(gameTime);
            player.Collision(gameobjects);

            // check collision for player

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            player.Draw(spriteBatch);

            foreach (var obj in gameobjects)
                obj.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void ScheduleDeletionOfObject(Gameobject obj)
        {
            toDelete.Add(obj);
        }

        private void DeleteScheduledObjects()
        {
            foreach (var obj in toDelete)
                gameobjects.Remove(obj);

            toDelete.Clear();

            if (gameobjects.Count == 0)
                SpawnPickableItems();
        }

        private void SpawnPickableItems()
        {
            Random rng = new Random();

            List<Vector2> takenPositions = new List<Vector2>();

            for (int i = 0; i < 10; ++i)
            {
                int x = rng.Next(60, 400);
                int y = rng.Next(60, 400);

                while (IsPositionTaken(takenPositions, x, y))
                {
                    x = rng.Next(60, 400);
                    y = rng.Next(60, 400);
                }

                takenPositions.Add(new Vector2(x, y));

                gameobjects.Add(new PickableItem(pickableItemTexture, new Vector2(x, y), new Vector2(25, 25), this));
            }
        }

        private bool IsPositionTaken(List<Vector2> list, int x, int y)
        {
            bool isTaken = false;

            foreach(var pos in list)
            {
                if ((pos.X >= x - 15 && pos.X <= x + 15) || (pos.Y >= y - 15 && pos.Y <= y + 15))
                    isTaken = true;
            }

            return isTaken;
        }
    }
}
