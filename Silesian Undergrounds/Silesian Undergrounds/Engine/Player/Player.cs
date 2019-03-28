using System.Collections.Generic;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace Silesian_Undergrounds.Engine.Player
{
    public class Player : AnimatedGameObject
    {

        // determines if the player is in 'attacking' mode (now just digging)
        bool attacking = false;

        public Player(Vector2 position, Vector2 size, int layer) : base(position, size, layer)
        {
            FramesPerSecond = 10;

            //Adds all the players animations
            // AddAnimation(int frames, int yPos, int xStartFrame, string name, int width, int height, Vector2 offset)
            // frames - number of frames of animation 
            // y position is a position from left right cornder
            // xStart frame is the x - sum of all widths
            // 80x80

            AddAnimation(5, 25, 313, "Down", 22, 22, new Vector2(0, 0));
            AddAnimation(1, 25, 313, "IdleDown", 22, 22, new Vector2(0, 0));

            //margins abovw and to the right/left are 25
            AddAnimation(5, 25, 25, "Up", 22, 22, new Vector2(0, 0));
            AddAnimation(1, 25, 25, "IdleUp", 22, 22, new Vector2(0, 0));

            AddAnimation(5, 25, 385, "Left", 22, 22, new Vector2(0, 0));
            AddAnimation(1, 25, 385, "IdleLeft", 22, 22, new Vector2(0, 0));

            AddAnimation(5, 25, 169, "Right", 22, 22, new Vector2(0, 0));
            AddAnimation(1, 25, 169, "IdleRight", 22, 22, new Vector2(0, 0));
            //Plays our start animation
            PlayAnimation("IdleDown");
        }

        // Loads content related to the player
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("minerCharacter");
        }

        public override void Update(GameTime gameTime)
        {
            sDirection = Vector2.Zero;

        
            HandleInput(Keyboard.GetState());

         
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

           
            sDirection *= speed;

            position += (sDirection * deltaTime);

            base.Update(gameTime);

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

        private void HandleInput(KeyboardState keyState)
        {
            if (!attacking)
            {
                if (keyState.IsKeyDown(Keys.W))
                {
                    sDirection += new Vector2(0, -1);
                    PlayAnimation("Up");
                    currentDirection = movementDirection.up;

                }
                if (keyState.IsKeyDown(Keys.A))
                {
                    sDirection += new Vector2(-1, 0);
                    PlayAnimation("Left");
                    currentDirection = movementDirection.left;

                }
                if (keyState.IsKeyDown(Keys.S))
                { 
                    sDirection += new Vector2(0, 1);
                    PlayAnimation("Down");
                    currentDirection = movementDirection.down;

                }
                if (keyState.IsKeyDown(Keys.D))
                {
                    sDirection += new Vector2(1, 0);
                    PlayAnimation("Right");
                    currentDirection = movementDirection.right;

                }
            }
      

        currentDirection = movementDirection.standstill;
            
        }

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack"))
            {
                Debug.WriteLine("Attack!");
                attacking = false;
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

        #region RectangleCollisionDetection

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
        #endregion
    }
}
