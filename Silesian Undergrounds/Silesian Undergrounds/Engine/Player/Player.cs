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

        public Player(Vector2 position, Vector2 size) : base(position, size)
        {
            FramesPerSecond = 10;

            //Adds all the players animations
            AddAnimation(12, 0, 0, "Down", 50, 50, new Vector2(0, 0));
            AddAnimation(1, 0, 0, "IdleDown", 50, 50, new Vector2(0, 0));
            AddAnimation(12, 50, 0, "Up", 50, 50, new Vector2(0, 0));
            AddAnimation(1, 50, 0, "IdleUp", 50, 50, new Vector2(0, 0));
            AddAnimation(8, 100, 0, "Left", 50, 50, new Vector2(0, 0));
            AddAnimation(1, 100, 0, "IdleLeft", 50, 50, new Vector2(0, 0));
            AddAnimation(8, 100, 8, "Right", 50, 50, new Vector2(0, 0));
            AddAnimation(1, 100, 8, "IdleRight", 50, 50, new Vector2(0, 0));
            AddAnimation(9, 150, 0, "AttackDown", 70, 80, new Vector2(0, 0));
            AddAnimation(9, 230, 0, "AttackUp", 70, 80, new Vector2(-13, -27));
            AddAnimation(9, 310, 0, "AttackLeft", 70, 70, new Vector2(-30, -5));
            AddAnimation(9, 380, 0, "AttackRight", 70, 70, new Vector2(+15, -5));
            //Plays our start animation
            PlayAnimation("IdleDown");
        }

        // Loads content related to the player
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("playerSheet");
        }


        public override void Update(GameTime gameTime)
        {
            //Move();
            //Makes the player stop moving when no key is pressed
            sDirection = Vector2.Zero;

            //Handles the users input
            HandleInput(Keyboard.GetState());

            //Calculates how many seconds that has passed since last iteration of Update
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Applies our speed to our direction
            sDirection *= speed;

            //Makes the movement framerate independent by multiplying with deltaTime
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
                    Debug.WriteLine("Key up!");
                    //Move char Up
                    sDirection += new Vector2(0, -1);
                    PlayAnimation("Up");
                    currentDirection = movementDirection.up;

                }
                if (keyState.IsKeyDown(Keys.A))
                {
                    //Move char Left
                    Debug.WriteLine("Key Left!");
                    sDirection += new Vector2(-1, 0);
                    PlayAnimation("Left");
                    currentDirection = movementDirection.left;

                }
                if (keyState.IsKeyDown(Keys.S))
                {
                    //Move char Down
                    Debug.WriteLine("Key Down!");
                    sDirection += new Vector2(0, 1);
                    PlayAnimation("Down");
                    currentDirection = movementDirection.down;

                }
                if (keyState.IsKeyDown(Keys.D))
                {
                    //Move char Right
                    Debug.WriteLine("Key Right!");
                    sDirection += new Vector2(1, 0);
                    PlayAnimation("Right");
                    currentDirection = movementDirection.right;

                }
            }
            if (keyState.IsKeyDown(Keys.Space))
            {
                if (currentAnimation.Contains("Down"))
                {
                    Debug.WriteLine("Space down!");
                    PlayAnimation("AttackDown");
                    attacking = true;
                    currentDirection = movementDirection.down;
                }
                if (currentAnimation.Contains("Left"))
                {
                    Debug.WriteLine("AttackLefts!");
                    PlayAnimation("AttackLeft");
                    attacking = true;
                    currentDirection = movementDirection.left;
                }
                if (currentAnimation.Contains("Right"))
                {
                    Debug.WriteLine("AttackRight!");
                    PlayAnimation("AttackRight");
                    attacking = true;
                    currentDirection = movementDirection.right;
                }
                if (currentAnimation.Contains("Up"))
                {
                    Debug.WriteLine("AttackUp!");
                    PlayAnimation("AttackUp");
                    attacking = true;
                    currentDirection = movementDirection.up;
                }
            }
            else if (!attacking)
            {
                if (currentAnimation.Contains("Left"))
                {
                    Debug.WriteLine("IdleLeft!");
                    PlayAnimation("IdleLeft");
                }
                if (currentAnimation.Contains("Right"))
                {
                    Debug.WriteLine("IdleRight!");
                    PlayAnimation("IdleRight");
                }
                if (currentAnimation.Contains("Up"))
                {
                    Debug.WriteLine("IdleUp!");
                    PlayAnimation("IdleUp");
                }
                if (currentAnimation.Contains("Down"))
                {
                    Debug.WriteLine("IdleDown!");
                    PlayAnimation("IdleDown");
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
