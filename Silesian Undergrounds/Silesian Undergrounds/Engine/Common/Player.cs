    using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Collisions;
using Silesian_Undergrounds.Engine.Behaviours;

namespace Silesian_Undergrounds.Engine.Common
{
    public class Player : AnimatedGameObject
    {
        public event EventHandler<PropertyChangedArgs<int>> MoneyChangeEvent = delegate { };
        public event EventHandler<PropertyChangedArgs<int>> KeyChangeEvent = delegate { };
        public event EventHandler<PropertyChangedArgs<int>> HungerChangeEvent = delegate { };
        public event EventHandler<PropertyChangedArgs<int>> LiveChangeEvent = delegate { };
        public event EventHandler<PropertyChangedArgs<int>> HungerMaxValueChangeEvent = delegate { };
        public event EventHandler<PropertyChangedArgs<int>> LiveMaxValueChangeEvent = delegate { };

        private Func<bool> OnPlayeDeath;
        private int HUNGER_DECREASE_INTERVAL_IN_SECONDS = 10;
        private int HUNGER_DECREASE_VALUE = 5;
        private const int LIVE_DECREASE_VALUE_WHEN_HUNGER_IS_ZERO = 20;
        private const int PLAYER_COLLIDER_BOX_WIDTH = 60;
        private const int PLAYER_COLLIDER_BOX_HEIGHT = 60;

        private float timeSinceHungerFall;

        private BoxCollider collider;

        private PlayerStatistic statistics;

        private PlayerBehaviour behaviour;

        public Player(Vector2 position, Vector2 size, int layer, Vector2 scale, PlayerStatistic globalPlayerStatistic) : base(position, size, layer, scale)
        {
            FramesPerSecond = 10;

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

            collider = new BoxCollider(this, PLAYER_COLLIDER_BOX_WIDTH, PLAYER_COLLIDER_BOX_HEIGHT, -2, -4, false);
            AddComponent(collider);
            statistics = globalPlayerStatistic;
            behaviour = new PlayerBehaviour(this);
            AddComponent(behaviour);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void SetOnDeath(Func<bool> functionOnDeath)
        {
            OnPlayeDeath += functionOnDeath;
        }

        public bool checkIfEnoughMoney(int cost)
        {
            if (cost > statistics.Money)
                return false;

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            sDirection = Vector2.Zero;

            HandleInput(Keyboard.GetState());

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!this.statistics.ImmuniteToHunger)
            {
                HandleHungerDecrasing(deltaTime);
            }

            sDirection *= speed;
            sDirection *= deltaTime;

            collider.Move(sDirection);

            base.Update(gameTime);
        }

        public void Initialize()
        {
            timeSinceHungerFall = 0;
        }

        public void AddMoney(int moneyToAdd)
        {
            MoneyAmount += moneyToAdd;
        }

        public void RemoveMoney(int moneyToRemove)
        {
            if (moneyToRemove > statistics.Money)
                MoneyAmount = 0;
            else
                MoneyAmount -= moneyToRemove;
        }

        public void AddKey(int keyNumbers)
        {
            KeyAmount += keyNumbers;
        }

        public void RefilHunger(int hungerValueToRefil)
        {
            if (statistics.Hunger + hungerValueToRefil > statistics.MaxHunger)
                HungerValue += (statistics.MaxHunger - statistics.Hunger);
            else
                HungerValue += hungerValueToRefil;
        }

        public bool CanRefilHunger(int hungerValueToRefil)
        {
            if (statistics.Hunger + hungerValueToRefil > statistics.MaxHunger)
                return false;

            return true;
        }


        public void RefilLive(int liveValueToRefil)
        {
            if (statistics.Health + liveValueToRefil > statistics.MaxHealth)
                LiveValue += (statistics.MaxHealth - statistics.Health);
            else
                LiveValue += liveValueToRefil;
        }

        public bool CanRefilLive(int liveValueToRefil)
        {
            if (statistics.Health + liveValueToRefil > statistics.MaxHealth)
                return false;

            return true;
        }

        public void RemoveKey(int numberKeysToRemove)
        {
            if (numberKeysToRemove > statistics.Key)
                KeyAmount = 0;
            else
                KeyAmount -= numberKeysToRemove;
        }

        public void DecreaseHungerValue(int hungerValueToDecrease)
        {
            if(statistics.Hunger > 0)
            {
                if (HungerValue >= hungerValueToDecrease)
                    HungerValue -= hungerValueToDecrease;
                else
                    HungerValue = 0;
            }
            else
            {
                DecreaseLiveValue(LIVE_DECREASE_VALUE_WHEN_HUNGER_IS_ZERO);
            }
        }

        public void DecreaseLiveValue(int liveValueToDecrease)
        {
            if(statistics.Health > 0)
            {
                if (LiveValue >= liveValueToDecrease)
                    LiveValue -= liveValueToDecrease;
                else
                    LiveValue = 0;
            }

            if (LiveValue <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Śmierć");
                OnPlayeDeath.Invoke();
                //TODO player die
            }
        }

        public void IncreaseLiveMaxValueBy(int liveMaxValueToIncrease)
        {
            MaxLiveValue = MaxLiveValue + liveMaxValueToIncrease;
        }

        public void IncreaseMovementSpped(float movementSpeedValueToIncrease)
        {
            this.statistics.MovementSpeed += movementSpeedValueToIncrease;
        }

        public void GrandPickupDouble()
        {
            this.statistics.PickupDouble = true;
        }

        public void GrandChestDropBooster()
        {
            this.statistics.ChestDropBooster = true;
        }

        public PlayerStatistic PlayerStatistic
        {
            get { return this.statistics;  }
        }

        public void IncreaseHungerMaxValueBy(int hungerMaxValueToIncrease)
        {
            MaxHungerValue = MaxHungerValue + hungerMaxValueToIncrease;
        }

        public void IncreaseAttackValueBy(float attackValueToIncrease)
        {
            this.statistics.AttackSpeed += attackValueToIncrease;
        }

        public int MaxHungerValue
        {
            get { return statistics.MaxHunger;  }
            private set
            {
                HungerMaxValueChangeEvent.Invoke(this, new PropertyChangedArgs<int>(statistics.MaxHealth, value));
                statistics.MaxHunger = value;
            }
        }

        public int MaxLiveValue
        {
            get { return statistics.MaxHealth;  }
            private set
            {
                LiveMaxValueChangeEvent.Invoke(this, new PropertyChangedArgs<int>(statistics.MaxHealth, value));
                statistics.MaxHealth = value;
            }
        }

        public int MoneyAmount
        {
            get { return statistics.Money; }
            private set
            {
                MoneyChangeEvent.Invoke(this, new PropertyChangedArgs<int>(statistics.Money, value));
                statistics.Money = value;
            }
        }

        public int KeyAmount
        {
            get { return statistics.Key; }
            private set
            {
                KeyChangeEvent.Invoke(this, new PropertyChangedArgs<int>(statistics.Key, value));
                statistics.Key = value;
            }
        }

        public int HungerValue
        {
            get { return statistics.Hunger; }
            private set
            {
                HungerChangeEvent.Invoke(this, new PropertyChangedArgs<int>(statistics.Hunger, value));
                statistics.Hunger = value;
            }
        }

        public int LiveValue
        {
            get { return statistics.Health; }
            private set
            {
                LiveChangeEvent.Invoke(this, new PropertyChangedArgs<int>(statistics.Health, value));
                statistics.Health = value;
            }
        }

        public int LiveMaxValue
        {
            get { return statistics.MaxHealth; }
        }

        public int HungerMaxValue
        {
            get { return statistics.MaxHunger; }
            private set
            {
                HungerMaxValueChangeEvent.Invoke(this, new PropertyChangedArgs<int>(statistics.MaxHunger, value));
                statistics.MaxHunger = value;
            }
        }

        private void HandleHungerDecrasing(float deltaTime)
        {
            timeSinceHungerFall += deltaTime;

            if (timeSinceHungerFall >= HUNGER_DECREASE_INTERVAL_IN_SECONDS)
            {
                DecreaseHungerValue(HUNGER_DECREASE_VALUE);
                timeSinceHungerFall = 0;
            }
        }

        private void HandleInput(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.W))
            {
                sDirection += new Vector2(0, -1 * this.statistics.MovementSpeed);
                PlayAnimation("Up");
                currentDirection = movementDirection.up;
                behaviour.SetOwnerOrientation(PlayerOrientation.ORIENTATION_NORTH);

            }
            if (keyState.IsKeyDown(Keys.A))
            {
                sDirection += new Vector2(-1 * this.statistics.MovementSpeed, 0);
                PlayAnimation("Left");
                currentDirection = movementDirection.left;
                behaviour.SetOwnerOrientation(PlayerOrientation.ORIENTATION_WEST);

            }
            if (keyState.IsKeyDown(Keys.S))
            {
                sDirection += new Vector2(0, 1 * this.statistics.MovementSpeed);
                PlayAnimation("Down");
                currentDirection = movementDirection.down;
                behaviour.SetOwnerOrientation(PlayerOrientation.ORIENTATION_SOUTH);

            }
            if (keyState.IsKeyDown(Keys.D))
            {
                sDirection += new Vector2(1 * this.statistics.MovementSpeed, 0);
                PlayAnimation("Right");
                currentDirection = movementDirection.right;
                behaviour.SetOwnerOrientation(PlayerOrientation.ORIENTATION_EAST);
            }

            currentDirection = movementDirection.standstill;
        }

        public override void AnimationDone(string animation)
        {
           if (animation.Contains("Attack"))
           {
               Debug.WriteLine("Attack!");
           } else if(IsAnimationMovement(animation))
           {
                currentAnimation = "Idle" + animation;
           }
        }

        // determines if current animation is up/donw/right/left
        private bool IsAnimationMovement(string animation)
        {
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
