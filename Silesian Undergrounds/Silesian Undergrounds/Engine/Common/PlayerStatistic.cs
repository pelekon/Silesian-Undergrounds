using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silesian_Undergrounds.Engine.Common {
    public class PlayerStatistic : StatisticHolder {

        private int moneyAmount;
        private int keyAmount;
        private int hungerValue;
        private int hungerMaxValue;
        private float hungerDecreaseInterval;
        private bool havePickupDouble = false;
        private bool haveChestDropBooster = false;
        private bool isImmuniteToHunger = false;
   

        public PlayerStatistic(int health, int maxHealth, int hunger, int maxHunger, float movementSpeed, float attackSpeed, int baseDamage, int moneyAmount, int keyAmount, float hungerDecreaseInterval) : base(health, maxHealth, movementSpeed, attackSpeed, baseDamage)
        {
            this.moneyAmount = moneyAmount;
            this.keyAmount = keyAmount;
            this.hungerValue = hunger;
            this.hungerMaxValue = maxHunger;
            this.hungerDecreaseInterval = hungerDecreaseInterval;
        }

        public int Hunger { get => hungerValue; set => hungerValue = value; }
        public int MaxHunger { get => hungerMaxValue; set => hungerMaxValue = value; }
        public int Key { get => keyAmount; set => keyAmount = value; }
        public int Money { get => moneyAmount; set => moneyAmount = value; }
        public bool PickupDouble { get => havePickupDouble; set => havePickupDouble = value; }
        public bool ChestDropBooster { get => haveChestDropBooster; set => haveChestDropBooster = value; }
        public bool ImmuniteToHunger { get => isImmuniteToHunger; set => isImmuniteToHunger = value; }
        public float HungerDecreaseInterval { get => hungerDecreaseInterval; set => hungerDecreaseInterval = value; }
    }
}
