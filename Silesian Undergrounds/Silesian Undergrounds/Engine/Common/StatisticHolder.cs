using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silesian_Undergrounds.Engine.Common
{
    public class StatisticHolder
    {
        private int health;
        private int maxHealth;
        private float movementSpeed;
        private float attackSpeed;
        private int baseDamage;

        public StatisticHolder(int health, int maxHealth, float movementSpeed, float attackSpeed, int baseDamage)
        {
            this.health = health;
            this.maxHealth = maxHealth;
            this.movementSpeed = movementSpeed;
            this.attackSpeed = attackSpeed;
            this.baseDamage = baseDamage;
        }

        public int Health { get => health; set => health = value; }
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
        public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
        public int BaseDamage { get => baseDamage; set => baseDamage = value; }
    }
}
