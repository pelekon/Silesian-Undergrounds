using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PdOne.Misc;
using System.Collections.Generic;
using System.Linq;

namespace PdOne
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        DrawableObject player;
        List<DrawableObject> objects = new List<DrawableObject>();
        SpriteFont font;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D holder;
            holder = Content.Load<Texture2D>("one");
            player = new DrawableObject(holder, 0, 0, 1f);

            holder = Content.Load<Texture2D>("two");
            DrawableObject obj = new DrawableObject(holder, 200, 200, 2f);
            objects.Add(obj);

            font = Content.Load<SpriteFont>("File");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float forceX = 0f;
            float forceY = 0f;

            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
                forceX = 1f;
            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
                forceX = -1f;

            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
                forceY = -1f;
            if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down))
                forceY = 1f;

            player.AddForce(forceX, forceY);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            DrawGameObjects();
            spriteBatch.End();


            base.Draw(gameTime);
        }

        private void DrawGameObjects()
        {
            spriteBatch.Draw(player.GetTexture(), player.GetPosition(), scale: player.GetScaleVector());

            foreach (var obj in objects)
                spriteBatch.Draw(obj.GetTexture(), obj.GetPosition(), scale: obj.GetScaleVector());

            if (CheckCollision())
                spriteBatch.DrawString(font, "There is collision", new Vector2(300, 0), Color.Red);
            else
                spriteBatch.DrawString(font, "There is no collision", new Vector2(300, 0), Color.Yellow);
        }

        private bool CheckCollision()
        {
            bool found = false;
            foreach (var obj in objects)
            {
                if (player.CheckForColision(obj))
                    found = true;
            }

            return found;
        }
    }
}
