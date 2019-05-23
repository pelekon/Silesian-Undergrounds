using Newtonsoft.Json;

namespace Silesian_Undergrounds.Engine.Config
{
    public class Config
    {
        [JsonProperty("player")]
        public PlayerConfig Player { get; set; } = new PlayerConfig();
        [JsonProperty("chest")]
        public ChestConfig Chest { get; set; } = new ChestConfig();
        [JsonProperty("pickable")]
        public PickableConfig Pickable { get; set; } = new PickableConfig();
        [JsonProperty("terrain")]
        public TerrainConfig Terrain { get; set; } = new TerrainConfig();
        
    }

    public class PlayerConfig
    {
        [JsonProperty("hunger-decrese-interval-in-seconds")]
        public float HungerDecreaseIntervalInSeconds { get; set; } = 10.0f;

        [JsonProperty("hunger-decrease-value")]
        public int HungerDecreaseValue { get; set; } = 5;

        [JsonProperty("live-decrease-value-when-hunger-is-zero")]
        public int LiveDecreaseValueWhenHungerIsZero { get; set; } = 20;

        [JsonProperty("player-collider-box-width")]
        public int PlayerColliderBoxWidth { get; set; } = 60;

        [JsonProperty("player-collider-box-height")]
        public int PlayerColliderBoxHeight { get; set; } = 60;

        [JsonProperty("health")]
        public int Health { get; set; } = 100;

        [JsonProperty("max-health")]
        public int MaxHealth { get; set; } = 150;

        [JsonProperty("hunger")]
        public int Hunger { get; set; } = 100;

        [JsonProperty("max-hunger")]
        public int MaxHunger { get; set; } = 150;

        [JsonProperty("attack-speed")]
        public float AttackSpeed { get; set; } = 1.0f;

        [JsonProperty("movement-speed")]
        public float MovementSpeed { get; set; } = 2.0f;

        [JsonProperty("key-amount")]
        public int KeyAmount { get; set; } = 0;

        [JsonProperty("money-amount")]
        public int MoneyAmount { get; set; } = 0;

        [JsonProperty("damage")]
        public int Damage { get; set; } = 10;
    }

    public class ChestConfig
    {
        [JsonProperty("number-of-chest-texture")]
        public int NumberOfChestTexture { get; set; } = 4;

        [JsonProperty("number-of-possible-spawned-item")]
        public int NumberOfPossibleSpawnedItem { get; set; } = 6;

        [JsonProperty("minimum-number-of-spawned-item")]
        public int MinimumNumberOfSpawnedItem { get; set; } = 1;

        [JsonProperty("range-of-spawn")]
        public int RangeOfSpawn { get; set; } = 1;
    }
    public class PickableConfig
    {
        [JsonProperty("heart")]
        public HeartConfig Heart { get; set; } = new HeartConfig();
        [JsonProperty("attack-booster")]
        public AttackBoosterConfig AttackBooster { get; set; } = new AttackBoosterConfig();
        [JsonProperty("hunger-booster")]
        public HungerBoosterConfig HungerBooster { get; set; } = new HungerBoosterConfig();
        [JsonProperty("live-booster")]
        public LiveBoosterConfig LiveBoosterConfig { get; set; } = new LiveBoosterConfig();
        [JsonProperty("movement-booster")]
        public MovementBoosterConfig MovementBoosterConfig { get; set; } = new MovementBoosterConfig();
        [JsonProperty("ore")]
        public OreConfig OreConfig{ get; set; } = new OreConfig();
    }

    public class HeartConfig
    {
        [JsonProperty("heart-increase-value")]
        public int HeartIncreaseValue { get; set; } = 10;

        [JsonProperty("live-regeneration-value")]
        public int LiveRegenerationValue { get; set; } = 25;
    }
    public class AttackBoosterConfig
    {
        [JsonProperty("player-attack-increase-by")]
        public float PlayerAttackIncreaseBy { get; set; } = 1.0f;
    }
    public class HungerBoosterConfig
    {
        [JsonProperty("player-max-hunger-value-increase-by")]
        public int PlayerMaxHungerValueIncreaseBy { get; set; } = 100;
    }
    public class LiveBoosterConfig
    {
        [JsonProperty("player-max-live-value-increase-by")]
        public int PlayerMaxLiveValueIncreaseBy { get; set; } = 100;
    }
    public class MovementBoosterConfig
    {
        [JsonProperty("player-movement-increase-by")]
        public float PlayerMovementIncreaseBy { get; set; } = 1.0f;
    }
    public class TerrainConfig
    {
        [JsonProperty("percentage-of-textures-with-things")]
        public int PercentageOfTexturesWithThings { get; set; } = 25;
    }
    public class OreConfig
    {
        [JsonProperty("coal-occurrence-percentage")]
        public int CoalOccurrencePercentage { get; set; } = 60;

        [JsonProperty("silver-occurrence-percentage")]
        public int SilverOccurrencePercentage { get; set; } = 30;

        [JsonProperty("gold-occurrence-percentage")]
        public int GoldOccurrencePercentage { get; set; } = 10;
    }
}