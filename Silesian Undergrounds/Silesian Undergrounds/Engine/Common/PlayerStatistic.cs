using Silesian_Undergrounds.Engine.Config;

namespace Silesian_Undergrounds.Engine.Common {
    public class PlayerStatistic : StatisticHolder {
        public PlayerStatistic(PlayerConfig playerConfig) : base(health: playerConfig.Health, maxHealth: playerConfig.MaxHealth, movementSpeed: playerConfig.MovementSpeed, attackSpeed: playerConfig.AttackSpeed, baseDamage: playerConfig.Damage)
        {
            Money = playerConfig.MoneyAmount;
            //Key = playerConfig.KeyAmount;
            Key = 500;
            Hunger = playerConfig.Hunger;
            MaxHunger = playerConfig.MaxHunger;
            HungerDecreaseInterval = playerConfig.HungerDecreaseIntervalInSeconds;
        }

        public int Hunger { get; set; }
        public int MaxHunger { get; set; }
        public int Key { get; set; }
        public int Money { get; set; }
        public bool PickupDouble { get; set; } = false;
        public bool ChestDropBooster { get; set; } = false;
        public bool ImmuniteToHunger { get; set; } = false;
        public float HungerDecreaseInterval { get; set; }
    }
}
