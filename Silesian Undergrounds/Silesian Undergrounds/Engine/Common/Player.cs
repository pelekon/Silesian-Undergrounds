using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Common
{
    public class Player : AnimatedGameObject
    {
        public event EventHandler<PropertyChangedArgs<int>> MoneyChangeEvent = delegate { };
        public event EventHandler<PropertyChangedArgs<int>> KeyChangeEvent = delegate { };
        public event EventHandler<PropertyChangedArgs<int>> HungerChangeEvent = delegate { };
        public event EventHandler<PropertyChangedArgs<int>> LiveChangeEvent = delegate { };

        // determines if the player is in 'attacking' mode (now just digging)
        bool attacking = false;
        
        private int moneyAmount;
        private int keyAmount;
        private int hungerValue;
        private int liveValue;

        private int maxHungerValue;
        private int maxLiveValue;

        BoxCollider collider;

        public Player(Vector2 position, Vector2 size, int layer, Vector2 scale) : base(position, size, layer, scale)
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

            collider = new BoxCollider(this, 65, 65, -2, -4, false);
            AddComponent(collider);
            collider.RegisterSelf();
        }

        ~Player()
        {
            foreach (var component in components)
                component.UnRegisterSelf();
        }

        public override void Update(GameTime gameTime)
        {
            sDirection = Vector2.Zero;
        
            HandleInput(Keyboard.GetState());

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            sDirection *= speed;
            sDirection *= deltaTime;

            collider.Move(sDirection);

            base.Update(gameTime);
        }

        public void Initialize()
        {
            moneyAmount = 0;
            keyAmount = 0;
            hungerValue = 100;
            liveValue = 100;
            maxHungerValue = 150;
            maxLiveValue = 100;
        }

        public void AddMoney(int moneyToAdd)
        {
            MoneyAmount += moneyToAdd;
        }

        public void RemoveMoney(int moneyToRemove)
        {
            if (moneyToRemove > moneyAmount)
                MoneyAmount = 0;
            else
                MoneyAmount -= moneyToRemove;
        }

        public void AddKey(int numberKeysToAdd)
        {
            KeyAmount += numberKeysToAdd;
        }

        public void RefilHunger(int hungerValueToRefil)
        {
            if (hungerValue + hungerValueToRefil > maxHungerValue)
                HungerValue += (maxHungerValue - hungerValue);
            else
                HungerValue += hungerValueToRefil;
        }

        public void RemoveKey(int numberKeysToRemove)
        {
            if (numberKeysToRemove > keyAmount)
                KeyAmount = 0;
            else
                KeyAmount -= numberKeysToRemove;
        }

        public int MaxHungerValue
        {
            get { return maxHungerValue;  }
        }

        public int MaxLiveValue
        {
            get { return maxLiveValue;  }
        }

        public int MoneyAmount
        {
            get { return moneyAmount; }
            private set
            {
                MoneyChangeEvent.Invoke(this, new PropertyChangedArgs<int>(moneyAmount, value));
                moneyAmount = value;
            }
        }

        public int KeyAmount
        {
            get { return keyAmount; }
            private set
            {
                KeyChangeEvent.Invoke(this, new PropertyChangedArgs<int>(keyAmount, value));
                keyAmount = value;
            }
        }

        public int HungerValue
        {
            get { return hungerValue; }
            private set
            {
                HungerChangeEvent.Invoke(this, new PropertyChangedArgs<int>(hungerValue, value));
                hungerValue = value;
            }
        }

        public int LiveValue
        {
            get { return liveValue; }
            private set
            {
                LiveChangeEvent.Invoke(this, new PropertyChangedArgs<int>(liveValue, value));
                liveValue = value;
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
           } else if(IsAnimationMovement(animation))
           {
                currentAnimation = "Idle" + animation;
                Debug.WriteLine(currentAnimation);
           }
           

        }

        // determines if current animation is up/donw/right/left
        private bool IsAnimationMovement(string animation)
        {
            // we all love Clean Code <3 
            if((animation.Contains("Up") || animation.Contains("Left") || animation.Contains("Down") || animation.Contains("Right")) && !animation.Contains("Idle")) return true;
           
            return false;
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
    }
}
