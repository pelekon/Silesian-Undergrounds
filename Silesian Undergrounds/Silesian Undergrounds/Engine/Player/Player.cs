using System.Collections.Generic;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Silesian_Undergrounds.Engine.Player
{
    class Player : Gameobject
    {
        public Player(Texture2D texture, Vector2 position, Vector2 size) : base(texture, position, size) { }

        public virtual void Update(GameTime gameTime)
        {
            Move();
        }

        // TODO: Remove this and split collisions to 2 sparate components:
        // Collision Box and Collider
        public void Collision(List<Gameobject> gameobjects)
        {
            foreach (Gameobject gameobject in gameobjects)
            {
                if (TouchingBottom(gameobject) || TouchingLeftSide(gameobject) || TouchingRightSide(gameobject) || TouchingTop(gameobject))
                {
                    gameobject.NotifyCollision(this);
                }
            }
        }

        private void Move()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left))
                AddForce(-1, 0);
            else if (state.IsKeyDown(Keys.Right))
                AddForce(1, 0);
            if (state.IsKeyDown(Keys.Up))
                AddForce(0, -1);
            else if (state.IsKeyDown(Keys.Down))
                AddForce(0, 1);
        }

        private bool TouchingLeftSide(Gameobject gameobjects)
        {
            return this.Rectangle.Right > gameobjects.Rectangle.Left &&
              this.Rectangle.Left < gameobjects.Rectangle.Left &&
              this.Rectangle.Bottom > gameobjects.Rectangle.Top &&
              this.Rectangle.Top < gameobjects.Rectangle.Bottom;
        }

        private bool TouchingRightSide(Gameobject gameobjects)
        {
            return this.Rectangle.Left < gameobjects.Rectangle.Right &&
              this.Rectangle.Right > gameobjects.Rectangle.Right &&
              this.Rectangle.Bottom > gameobjects.Rectangle.Top &&
              this.Rectangle.Top < gameobjects.Rectangle.Bottom;
        }

        private bool TouchingTop(Gameobject gameobjects)
        {
            return this.Rectangle.Bottom > gameobjects.Rectangle.Top &&
              this.Rectangle.Top < gameobjects.Rectangle.Top &&
              this.Rectangle.Right > gameobjects.Rectangle.Left &&
              this.Rectangle.Left < gameobjects.Rectangle.Right;
        }

        private bool TouchingBottom(Gameobject gameobjects)
        {
            return this.Rectangle.Top < gameobjects.Rectangle.Bottom &&
              this.Rectangle.Bottom > gameobjects.Rectangle.Bottom &&
              this.Rectangle.Right > gameobjects.Rectangle.Left &&
              this.Rectangle.Left < gameobjects.Rectangle.Right;
        }
    }
}
