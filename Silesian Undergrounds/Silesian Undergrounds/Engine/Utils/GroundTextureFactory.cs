using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Engine.Item;

namespace Silesian_Undergrounds.Engine.Utils
{
    internal class GroundTextureFactory
    {
        private const int BasicTextureIndex = 1;
        private const int StartWithThingsTextureIndex = 2;
        private const int EndWithThingsTextureIndex = 25;
        private const int PercentageOfTexturesWithThings = 25;
        private static GroundEnum RandGround(Random random) => random.Next(1, 100) >= PercentageOfTexturesWithThings ? GroundEnum.Basic : GroundEnum.WithThings;
        public static List<Ground> ScenePickableItemsFactory(List<GameObject> positionSources)
        {
            var list = new List<Ground>();
            var random = new Random();
            foreach(var source in positionSources)
            {
                var itemType = RandGround(random);
                switch (itemType)
                {
                    case GroundEnum.Basic:
                        list.Add(GroundFactory(BasicTextureIndex, source.position, source.size));
                        break;
                    case GroundEnum.WithThings:
                        var textureNumber = random.Next(StartWithThingsTextureIndex, EndWithThingsTextureIndex);
                        list.Add(GroundFactory(textureNumber, source.position, source.size));
                        break;
                    default:
                        #if DEBUG
                        Console.WriteLine("Not registered GroundTexture Type in GroundTextureFactory!");
                        #endif
                        break;
                }
            }

            return list;
        }
        public static Ground GroundFactory(int textureNumber, Vector2 position, Vector2 size) => new Ground(TextureMgr.Instance.GetTexture("Items/Grounds/ground_" + textureNumber), position, size, 3, null);
    }
}
