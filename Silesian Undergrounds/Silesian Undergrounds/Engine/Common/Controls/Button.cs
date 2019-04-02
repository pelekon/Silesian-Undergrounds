using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Silesian_Undergrounds.Engine.Common;
using System.Diagnostics;

namespace Silesian_Undergrounds.States.Controls
{
    class Button : GameObject
    {

        public string Text { get; }
        private Func<Button, Boolean> Callback;
        private Texture2D ButtonNotClickedTexture;
        private Texture2D ButtonHoveringTexture;
        private SpriteFont ButtonTextFont;

        // public GameObject(Texture2D texture, Vector2 position, Vector2 size, int layer, Vector2? scale = null)
        public Button(string text, Texture2D buttonNotClicked, Texture2D buttonClicked, Vector2 Position, Vector2 Size, SpriteFont ButtonTextFont, Vector2? scale = null) : base(texture: buttonNotClicked,position: Position, size: Size, scale: scale)
        {
            this.Text = text;
            this.ButtonHoveringTexture = buttonClicked;
            this.ButtonNotClickedTexture = buttonNotClicked;
            this.ButtonTextFont = ButtonTextFont;
        }

        public void SetOnClickCallback(Func<Button, Boolean> callback)
        {
            this.Callback = callback;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            //&& mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed
            this.texture = IsMouseInsideButton() ? ButtonHoveringTexture : ButtonNotClickedTexture;
        }


        private Boolean IsMouseInsideButton()
        {
            MouseState mouseState = Mouse.GetState();

            
            if (mouseState.X < position.X + size.X &&
                   mouseState.X > size.X &&
                   mouseState.Y < size.Y + position.Y &&
                   mouseState.Y > size.Y)
            {
                Debug.WriteLine("Inside the buttonMenu!");
                return true;
            }

            Debug.WriteLine("Outside the buttonMenu!");
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            DrawString(spriteBatch);
        }

        // draws text to fit specific boundires (Rectangle)
        private void DrawString(SpriteBatch spriteBatch)
        {
            Vector2 size = ButtonTextFont.MeasureString(Text);

            float xScale = (Rectangle.Width / size.X);
            float yScale = (Rectangle.Height / size.Y);
            
            float scale = Math.Min(xScale, yScale);

            
            int strWidth = (int)Math.Round(size.X * scale);
            int strHeight = (int)Math.Round(size.Y * scale);
            Vector2 position = new Vector2();
            position.X = (((Rectangle.Width - strWidth) / 2) + Rectangle.X);
            position.Y = (((Rectangle.Height - strHeight) / 2) + Rectangle.Y);

           
            float rotation = 0.0f;
            Vector2 spriteOrigin = new Vector2(0, 0);
            float spriteLayer = 0.0f; 
            SpriteEffects spriteEffects = new SpriteEffects();

           
            spriteBatch.DrawString(ButtonTextFont, Text, position, Color.Black, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        }


    }
}
