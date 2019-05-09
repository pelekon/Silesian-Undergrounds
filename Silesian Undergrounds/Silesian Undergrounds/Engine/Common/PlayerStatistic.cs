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
        private bool havePickupDouble = false;
   

        public PlayerStatistic(int health, int maxHealth, int hunger, int maxHunger, float movementSpeed, float attackSpeed, int baseDamage, int moneyAmount, int keyAmount) : base(health, maxHealth, movementSpeed, attackSpeed, baseDamage)
        {
            this.moneyAmount = moneyAmount;
            this.keyAmount = keyAmount;
            this.hungerValue = hunger;
            this.hungerMaxValue = maxHunger;
        }

        public int Hunger { get => hungerValue; set => hungerValue = value; }
        public int MaxHunger { get => hungerMaxValue; set => hungerMaxValue = value; }
        public int Key { get => keyAmount; set => keyAmount = value; }
        public int Money { get => moneyAmount; set => moneyAmount = value; }
        public bool PickupDouble { get => havePickupDouble; set => havePickupDouble = value; }
    }
}
