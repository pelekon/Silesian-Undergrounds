using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PracaDomowaPgWyklad {
    class Character {
        protected Texture2D texture;
        public Vector2 position;
        public Vector2 size;

        public Rectangle Rectangle
            {
                get
                {
                return new Rectangle((int)position.X, (int)position.Y, (int) size.X, (int) size.Y);
                }
            }

        public Character(Texture2D texture)
        {
            this.texture = texture;
        }

        public virtual void Update(GameTime gameTime)
        {
            Move();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: texture, destinationRectangle: Rectangle);
        }

        public bool TouchingLeftSide(Character character)
        {
            return this.Rectangle.Right > character.Rectangle.Left &&
              this.Rectangle.Left < character.Rectangle.Left &&
              this.Rectangle.Bottom > character.Rectangle.Top &&
              this.Rectangle.Top < character.Rectangle.Bottom;
        }

        public bool TouchingRightSide(Character character)
        {
            return this.Rectangle.Left < character.Rectangle.Right &&
              this.Rectangle.Right > character.Rectangle.Right &&
              this.Rectangle.Bottom > character.Rectangle.Top &&
              this.Rectangle.Top < character.Rectangle.Bottom;
        }

        public bool TouchingTop(Character character)
        {
            return this.Rectangle.Bottom > character.Rectangle.Top &&
              this.Rectangle.Top < character.Rectangle.Top &&
              this.Rectangle.Right > character.Rectangle.Left &&
              this.Rectangle.Left < character.Rectangle.Right;
        }

        public bool TouchingBottom(Character character)
        {
            return this.Rectangle.Top < character.Rectangle.Bottom &&
              this.Rectangle.Bottom > character.Rectangle.Bottom &&
              this.Rectangle.Right > character.Rectangle.Left &&
              this.Rectangle.Left < character.Rectangle.Right;
        }

        private void Move()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left))
                position.X -= 1;
            else if (state.IsKeyDown(Keys.Right))
                position.X += 1;

            if (state.IsKeyDown(Keys.Up))
                position.Y -= 1;
            else if (state.IsKeyDown(Keys.Down))
                position.Y += 1;
        }


    }
}
