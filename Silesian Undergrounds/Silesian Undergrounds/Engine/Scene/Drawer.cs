using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Silesian_Undergrounds.Engine.Scene
{
    public class Drawer
    {
        private Drawer() {}
        private static SpriteBatch _spriteBatch;
        private static GameTime _gameTime;

        public static class Shaders
        {
            private static Effect _pickUpEffect, _grayScaleEffect;

            public static void DrawPickUpEffect(Action<SpriteBatch, GameTime> drawer, Matrix? transformMatrix = null)
            {
                _pickUpEffect.Parameters["gameTime"].SetValue((float)_gameTime.TotalGameTime.TotalMilliseconds);
                _spriteBatch.Begin(transformMatrix: transformMatrix, effect: _pickUpEffect);
                drawer.Invoke(_spriteBatch, _gameTime);
                _spriteBatch.End();
            }

            public static void DrawGrayScaleEffect(Action<SpriteBatch, GameTime> drawer, Matrix? transformMatrix = null)
            {
                _spriteBatch.Begin(transformMatrix: transformMatrix, effect: _grayScaleEffect);
                drawer.Invoke(_spriteBatch, _gameTime);
                _spriteBatch.End();
            }

            internal static void LoadShaders(ContentManager content)
            {
                _pickUpEffect = content.Load<Effect>("PickUpShader");
                _grayScaleEffect = content.Load<Effect>("GrayScaleShader");
            }

        }

        public static void Draw(Action<SpriteBatch, GameTime> drawer, Matrix? transformMatrix = null)
        {
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            drawer.Invoke(_spriteBatch, _gameTime);
            _spriteBatch.End();
        }

        public static void UpdateGameTime(GameTime gameTime)
        {
            _gameTime = gameTime;
        }

        public static void Initialize(SpriteBatch spriteBatch, ContentManager content)
        {
            _spriteBatch = spriteBatch;
            Shaders.LoadShaders(content);
        }
    }
}