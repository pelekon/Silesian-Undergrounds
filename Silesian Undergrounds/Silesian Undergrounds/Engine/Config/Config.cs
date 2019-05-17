using Newtonsoft.Json;

namespace Silesian_Undergrounds.Engine.Config
{
    public class Config
    {
        [JsonProperty("player", Required = Required.Always)]
        public PlayerConfig Player { get; set; }
        [JsonProperty("chest", Required = Required.Always)]
        public ChestConfig Chest { get; set; }
        [JsonProperty("pickable", Required = Required.Always)]
        public PickableConfig Pickable { get; set; }
    }

    public class PlayerConfig
    {
        [JsonProperty("player-speed", Required = Required.Always)]
        public int PlayerSpeed { get; set; }
        [JsonProperty("hunger-decrese-interval-in-seconds", Required = Required.Always)]
        public int HungerDecreaseIntervalInSeconds { get; set; }
        [JsonProperty("hunger-decrease-value", Required = Required.Always)]
        public int HungerDecreaseValue { get; set; }
        [JsonProperty("live-decrease-value-when-hunger-is-zero", Required = Required.Always)]
        public int LiveDecreaseValueWhenHungerIsZero { get; set; }
        [JsonProperty("player-collider-box-width", Required = Required.Always)]
        public int PlayerColliderBoxWidth { get; set; }
        [JsonProperty("player-collider-box-height", Required = Required.Always)]
        public int PlayerColliderBoxHeight { get; set; }
    }

    public class ChestConfig
    {
        [JsonProperty("number-of-chest-texture", Required = Required.Always)]
        public int NumberOfChestTexture { get; set; }
        [JsonProperty("number-of-possible-spawned-item", Required = Required.Always)]
        public int NumberOfPossibleSpawnedItem { get; set; }
        [JsonProperty("minimum-number-of-spawned-item", Required = Required.Always)]
        public int MinimumNumberOfSpawnedItem { get; set; }
        [JsonProperty("range-of-spawn", Required = Required.Always)]
        public int RangeOfSpawn { get; set; }
    }
    public class PickableConfig
    {
        [JsonProperty("heart", Required = Required.Always)]
        public HeartConfig Heart { get; set; }
        [JsonProperty("attack-booster", Required = Required.Always)]
        public AttackBoosterConfig AttackBooster { get; set; }
        [JsonProperty("hunger-booster", Required = Required.Always)]
        public HungerBoosterConfig HungerBooster { get; set; }
        [JsonProperty("live-booster", Required = Required.Always)]
        public LiveBoosterConfig LiveBoosterConfig { get; set; }
    }

    public class HeartConfig
    {
        [JsonProperty("heart-increase-value", Required = Required.Always)]
        public int HeartIncreaseValue { get; set; }
        [JsonProperty("live-regeneration-value", Required = Required.Always)]
        public int LiveRegenerationValue { get; set; }
    }
    public class AttackBoosterConfig
    {
        [JsonProperty("player-attack-increase-by", Required = Required.Always)]
        public float PlayerAttackIncreaseBy { get; set; }
    }
    public class HungerBoosterConfig
    {
        [JsonProperty("player-max-hunger-value-increase-by", Required = Required.Always)]
        public int PlayerMaxHungerValueIncreaseBy { get; set; }
    }
    public class LiveBoosterConfig
    {
        [JsonProperty("player-max-live-value-increase-by", Required = Required.Always)]
        public int PlayerMaxLiveValueIncreaseBy { get; set; }
    }
}