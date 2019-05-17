using System.Collections.Generic;
using Newtonsoft.Json;
using Silesian_Undergrounds.Engine.Scene;

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
    }

    public class HeartConfig
    {
        [JsonProperty("heart-increase-value", Required = Required.Always)]
        public int HeartIncreaseValue { get; set; }
        [JsonProperty("live-regeneration-value", Required = Required.Always)]
        public int LiveRegenerationValue { get; set; }
    }
}