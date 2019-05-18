using System;
using System.IO;
using Newtonsoft.Json;

namespace Silesian_Undergrounds.Engine.Config
{
    public class ConfigMgr
    {

        private ConfigMgr() {}
        private static Config _config;
        public static Config Config => _config;
        public static PlayerConfig PlayerConfig => _config.Player;
        public static ChestConfig ChestConfig => _config.Chest;
        public static PickableConfig PickableConfig => _config.Pickable;
        public static HeartConfig HeartConfig => _config.Pickable.Heart;
        public static AttackBoosterConfig AtackBoosterConfig => _config.Pickable.AttackBooster;
        public static HungerBoosterConfig HungerBoosterConfig => _config.Pickable.HungerBooster;
        public static LiveBoosterConfig LiveBoosterConfig => _config.Pickable.LiveBoosterConfig;
        public static MovementBoosterConfig MovementBoosterConfig => _config.Pickable.MovementBoosterConfig;
        public static TerrainConfig TerrainConfig => _config.Terrain;

        public static Config LoadConfig()
        {
            _config = ReadConfigFile() ?? new Config();
            return _config;
        }

        private static Config ReadConfigFile()
        {
            var configFilePath = Path.Combine(Constants.DataDirectory, Constants.ConfigFileName);
            var fileExists = File.Exists(configFilePath);
            if (!fileExists) return null;
           Config configFile;
            using (var file = File.OpenText(configFilePath))
            {
                try { configFile = (Config)new JsonSerializer().Deserialize(file, typeof(Config)); }
                catch (JsonException e)
                {
                    #if DEBUG
                        Console.WriteLine(e.Message);
                    #endif
                    return null;
                }
            }
            return configFile;
        }
    }
}