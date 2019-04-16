using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Engine.Item;

namespace Silesian_Undergrounds.Engine.Utils
{
    class GroundTextureFactory
    {
        private static GroundEnum RandGround(Random random)
        {
            int randed = random.Next(1, 100);
            if (randed <= 10)
                return GroundEnum.Basic;
            else if (randed > 10 && randed <= 25)
                return GroundEnum.Basic;
            else
                return GroundEnum.Basic;
        }
        public static List<Ground> ScenePickableItemsFactory(List<GameObject> positionSources)
        {
            List<Ground> list = new List<Ground>();
            Random random = new Random();
            foreach(var source in positionSources)
            {

                GroundEnum itemType = RandGround(random);
                if (itemType == GroundEnum.None)
                    continue;

                switch(itemType)
                {
                    case GroundEnum.Basic:
                        list.Add(GroundFactory(random, source.position, source.size));
                        break;
                    default:
                        #if DEBUG
                        Console.WriteLine("Not registered PickableItem Type in ScenePickableItemsFactory!");
                        #endif
                        break;
                }
            }

            return list;
        }
        public static Ground GroundFactory(Random random, Vector2 position, Vector2 size)
        {
            int textureNumber = random.Next(1, 14);
            return new Ground(TextureMgr.Instance.GetTexture("Items/Grounds/ground_" + textureNumber), position, size, 3, null);
        }
    }
}
