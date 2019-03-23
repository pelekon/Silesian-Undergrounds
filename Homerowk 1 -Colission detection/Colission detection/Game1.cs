using Colission_detection.Models;
using Colission_detection.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Colission_detection
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    { 
        private List<Sprite> _sprites;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public bool isBackgroundGreenColor = true;
        Color backgroundColor = Color.LawnGreen;


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

            // the player is also the rectangle ;)
            var playerTexture = Content.Load<Texture2D>("trog_top");
            var obstacleTexture = Content.Load<Texture2D>("floor");

            // initialize all sprites in the 'game'
            _sprites = new List<Sprite>()
            {
                new GamePawn(playerTexture, new Input()
                  {
                    Left = Keys.Left,
                    Right = Keys.Right,
                    Up = Keys.Up,
                    Down = Keys.Down,
                  }, 5)
                        {
                          Position = new Vector2(300, 100),
                          TextureColor = Color.Green
                        },
                new Obstacle(obstacleTexture)
                        {
                            Position = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2, GraphicsDevice.Viewport.Bounds.Height / 2)
        }
              };
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
          
            foreach (var sprite in _sprites)
                sprite.Update(gameTime, _sprites, this);
           

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            this.backgroundColor = this.isBackgroundGreenColor ? Color.LawnGreen : Color.Red;
            GraphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin();
           
            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);

            // draw obstacle in the center of the window
            //spriteBatch.Draw(textureSecondRectangle, screenCenter, null, Color.White, 0f, textureCenter, 1f, SpriteEffects.None, 1f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
