using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silesian_Undergrounds.Engine.Common {
    class PlayerStatistic : StatisticHolder {

        private int moneyAmount;
        private int keyAmount;
        private int hungerValue;
   

        public PlayerStatistic(int health, int maxHealth, float movementSpeed, float attackSpeed, int baseDamage, int moneyAmount, int keyAmount, int hungerValue) : base(health, maxHealth, movementSpeed, attackSpeed, baseDamage)
        {
            this.moneyAmount = moneyAmount;
            this.keyAmount = keyAmount;
            this.hungerValue = hungerValue;
        }



    }
}
