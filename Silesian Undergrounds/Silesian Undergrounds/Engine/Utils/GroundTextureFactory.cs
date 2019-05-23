using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Engine.Item;
using Silesian_Undergrounds.Engine.Config;

namespace Silesian_Undergrounds.Engine.Utils
{
    internal class GroundTextureFactory
    {
        private const string isRandomTextureName = "../Content/kolejne";
        private const int BasicTextureIndex = 1;
        private const int StartWithThingsTextureIndex = 2;
        private const int EndWithThingsTextureIndex = 25;
        private static GroundEnum RandGround(Random random) => random.Next(1, 100) >= ConfigMgr.TerrainConfig.PercentageOfTexturesWithThings ? GroundEnum.Basic : GroundEnum.WithThings;
        public static List<Ground> GroundFactory(List<GameObject> positionSources)
        {
            var list = new List<Ground>();
            var random = new Random();
            foreach(var source in positionSources)
            {
                Console.WriteLine("Name: " + source.texture.Name);
                switch(source.texture.Name)
                {
                    case isRandomTextureName:
                        list.Add(GetRandomGround(random, source));
                        break;
                    default:
                        list.Add(new Ground(TextureMgr.Instance.GetTexture(source.texture.Name), source.position, source.size, 2, null));
                        break;
                }
                
            }
            return list;
        }
        static Ground GetRandomGround(Random random, GameObject source)
        {
            var itemType = RandGround(random);
            switch (itemType)
            {
                case GroundEnum.Basic:
                    return GroundFactory(BasicTextureIndex, source.position, source.size);
                case GroundEnum.WithThings:
                    var textureNumber = random.Next(StartWithThingsTextureIndex, EndWithThingsTextureIndex);
                    return GroundFactory(textureNumber, source.position, source.size);
                default:
                    #if DEBUG
                    Console.WriteLine("Not registered GroundTexture Type in GroundTextureFactory!");
#endif
                    return null;
            }

        }
        public static Ground GroundFactory(int textureNumber, Vector2 position, Vector2 size) => new Ground(TextureMgr.Instance.GetTexture("Items/Grounds/ground_" + textureNumber), position, size, 2, null);
    }
}
