using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Scene;

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

        public static Config LoadConfig()
        {
            _config = ReadConfigFile();
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
                catch (Newtonsoft.Json.JsonSerializationException e)
                {
                    #if DEBUG
                        Console.WriteLine(e.Message);
                    #endif
                    return null;
                }
            }
            Console.WriteLine("PlayerSpeed: " + configFile.Player.PlayerSpeed);
            return configFile;
        }
    }
}