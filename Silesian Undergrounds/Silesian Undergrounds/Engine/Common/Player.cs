    using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Collisions;
using Silesian_Undergrounds.Engine.Behaviours;
using Silesian_Undergrounds.Engine.Config;
using Silesian_Undergrounds.Engine.Components;
using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Engine.Common
{
    public class Player : GameObject
    {
        public event EventHandler<PropertyChangedArgs<int>> MoneyChangeEvent = delegate { };
        public event EventHandler<PropertyChangedArgs<int>> KeyChangeEvent = delegate { };
        public event EventHandler<PropertyChangedArgs<int>> HungerChangeEvent = delegate { };
        public event EventHandler<PropertyChangedArgs<int>> LiveChangeEvent = delegate { };
        public event EventHandler<PropertyChangedArgs<int>> HungerMaxValueChangeEvent = delegate { };
        public event EventHandler<PropertyChangedArgs<int>> LiveMaxValueChangeEvent = delegate { };

        private Func<bool> OnPlayeDeath;
        private Boolean isPlayerMoving = false;

        private float timeSinceHungerFall;

        private BoxCollider collider;
        private PlayerStatistic statistics;
        private PlayerBehaviour behaviour;
        private Animator animator;
        private Vector2 sDirection;
        private float playerVisibility = 0.0f;
        public float PlayerVisiblity
        {
            get
            {
                return playerVisibility;
            }

            set
            {
                if(!(value > 1.0f || value < 0.0f))
                {
                    playerVisibility = value;
                }
            }
        }


        private readonly int textureSpacingX = 20;
        private readonly int textureSpacingY = 24;

        public Player(Vector2 position, Vector2 size, int layer, Vector2 scale, PlayerStatistic globalPlayerStatistic) : base(null, position, size, layer, scale)
        {
            // SetUp texture
            TextureMgr.Instance.LoadSingleTextureFromSpritescheet("minerCharacter", "PlayerTexture", 13, 6, 0, 4, textureSpacingX, textureSpacingY);
            texture = TextureMgr.Instance.GetTexture("PlayerTexture");

            collider = new BoxCollider(this, ConfigMgr.PlayerConfig.PlayerColliderBoxWidth, ConfigMgr.PlayerConfig.PlayerColliderBoxHeight, -2, -4, false);
            AddComponent(collider);
            statistics = globalPlayerStatistic;
            behaviour = new PlayerBehaviour(this);
            AddComponent(behaviour);
            sDirection = Vector2.Zero;
            animator = new Animator(this);
            AddComponent(animator);
            LoadAndSetUpAnimations();
            speed = 50f;
            #if DEBUG
            statistics.MovementSpeed = 6.0f;
            #endif
            ChangeDrawAbility(false);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.color = new Color(Color.White, this.PlayerVisiblity);

            base.Draw(spriteBatch);
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

        public void ChangerHungerDecreaseIntervalBy(float percentToChange)
        {
            this.statistics.HungerDecreaseInterval *= percentToChange;
        }

        public override void Update(GameTime gameTime)
        {
            sDirection = Vector2.Zero;

            if(isPlayerMoving)
                HandleInput(Keyboard.GetState());

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!this.statistics.ImmuniteToHunger)
            {
                HandleHungerDecrasing(deltaTime);
            }

            if (isPlayerMoving)
            {
                sDirection *= speed;
                sDirection *= deltaTime;

                collider.Move(sDirection);
            }

            base.Update(gameTime);
        }

        public void CanMove(Boolean canMove)
        {
            this.isPlayerMoving = canMove;
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
                DecreaseLiveValue(ConfigMgr.PlayerConfig.LiveDecreaseValueWhenHungerIsZero);
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
                OnPlayeDeath.Invoke();
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

            if (timeSinceHungerFall >= this.statistics.HungerDecreaseInterval)
            {
                DecreaseHungerValue(ConfigMgr.PlayerConfig.HungerDecreaseValue);
                timeSinceHungerFall = 0;
            }
        }

        private void HandleInput(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.W))
            {
                sDirection += new Vector2(0, -1 * this.statistics.MovementSpeed);
                animator.PlayAnimation("MoveUp");
                behaviour.SetOwnerOrientation(PlayerOrientation.ORIENTATION_NORTH);

            }
            if (keyState.IsKeyDown(Keys.A))
            {
                sDirection += new Vector2(-1 * this.statistics.MovementSpeed, 0);
                animator.PlayAnimation("MoveLeft");
                behaviour.SetOwnerOrientation(PlayerOrientation.ORIENTATION_WEST);

            }
            if (keyState.IsKeyDown(Keys.S))
            {
                sDirection += new Vector2(0, 1 * this.statistics.MovementSpeed);
                animator.PlayAnimation("MoveDown");
                behaviour.SetOwnerOrientation(PlayerOrientation.ORIENTATION_SOUTH);

            }
            if (keyState.IsKeyDown(Keys.D))
            {
                sDirection += new Vector2(1 * this.statistics.MovementSpeed, 0);
                animator.PlayAnimation("MoveRight");
                behaviour.SetOwnerOrientation(PlayerOrientation.ORIENTATION_EAST);
            }
        }

        private void LoadAndSetUpAnimations()
        {
            // Load necessary textures
            TextureMgr.Instance.LoadAnimationFromSpritesheet("minerCharacter", "PlayerMoveUp", 13, 6, 0, 5, textureSpacingX, textureSpacingY, false, true);
            TextureMgr.Instance.LoadAnimationFromSpritesheet("minerCharacter", "PlayerMoveDown", 13, 6, 4, 5, textureSpacingX, textureSpacingY, false, true);
            TextureMgr.Instance.LoadAnimationFromSpritesheet("minerCharacter", "PlayerMoveLeft", 13, 6, 5, 5, textureSpacingX, textureSpacingY, false, true);
            TextureMgr.Instance.LoadAnimationFromSpritesheet("minerCharacter", "PlayerMoveRight", 13, 6, 2, 5, textureSpacingX, textureSpacingY, false, true);

            animator.AddAnimation("MoveUp", TextureMgr.Instance.GetAnimation("PlayerMoveUp"), 1000, false, true);
            animator.AddAnimation("MoveDown", TextureMgr.Instance.GetAnimation("PlayerMoveDown"), 1000, false, true);
            animator.AddAnimation("MoveLeft", TextureMgr.Instance.GetAnimation("PlayerMoveLeft"), 1000, false, true);
            animator.AddAnimation("MoveRight", TextureMgr.Instance.GetAnimation("PlayerMoveRight"), 1000, false, true);
        }
    }
}
