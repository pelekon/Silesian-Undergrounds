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
        //private Texture2D CurrentTexture;

        // public GameObject(Texture2D texture, Vector2 position, Vector2 size, int layer, Vector2? scale = null)
        public Button(string text, Texture2D buttonNotClicked, Texture2D buttonClicked, Vector2 Position, Vector2 Size) : base(texture: buttonNotClicked,position: Position, size: Size)
        {
            this.Text = text;
            this.ButtonHoveringTexture = buttonClicked;
            this.ButtonNotClickedTexture = buttonNotClicked;
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
                Debug.WriteLine("Inside the button!");
                return true;
            }

            Debug.WriteLine("Outside the button!");
            return false;
        }


    }
}
