using System;
using System.Collections.Generic;

using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Engine.Item;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework;

namespace Silesian_Undergrounds.Engine.Utils
{
    public sealed class GameObjectFactory
    {
        #region OBJECT_TYPE_RAND_FUNCTIONS

        private static OreEnum RandOreType(Random random)
        {
            int randed = random.Next(1, 100);
            if (randed > 40 && randed <= 70)
                return OreEnum.Coal;
            else if (randed > 70 && randed <= 90)
                return OreEnum.Silver;
            else
                return OreEnum.Gold;

        }

        private static FoodEnum RandFoodType(Random random)
        {
            int randed = random.Next(1, 100);

            if (randed <= 65)
                return FoodEnum.Meat;
            else
                return FoodEnum.Steak;

        }

        private static PickableEnum RandItem(Random random)
        {
            int randed = random.Next(1, 100);
            if (randed <= 10)
                return PickableEnum.None;
            else if (randed > 10 && randed <= 25)
                return PickableEnum.Hearth;
            else if (randed > 25 && randed <= 45)
                return PickableEnum.Food;
            else if (randed > 45 && randed <= 75)
                return PickableEnum.Ore;
            else if (randed > 75 && randed <= 85)
                return PickableEnum.Chest;
            else
                return PickableEnum.Key;
        }

        #endregion

        public static List<PickableItem> ScenePickableItemsFactory(List<GameObject> positionSources, Scene.Scene scene)
        {
            List<PickableItem> list = new List<PickableItem>();
            Random random = new Random();

            foreach(var source in positionSources)
            {

                PickableEnum itemType = RandItem(random);
                if (itemType == PickableEnum.None)
                    continue;

                switch(itemType)
                {
                    case PickableEnum.Ore:
                        list.Add(OreFactory(random, source.position, source.size, scene));
                        break;
                    case PickableEnum.Chest:
                        list.Add(ChestFactory(source.position, source.size, scene));
                        break;
                    case PickableEnum.Food:
                        list.Add(FoodFactory(random, source.position, source.size, scene));
                        break;
                    case PickableEnum.Key:
                        list.Add(KeyFactory(source.position, source.size, scene));
                        break;
                    case PickableEnum.Hearth:
                        list.Add(HeartFactory(source.position, source.size, scene));
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

        public static Ore OreFactory(Random rng, Vector2 position, Vector2 size, Scene.Scene scene)
        {
            OreEnum type = RandOreType(rng);
            int textureNumber = rng.Next(1, 3);

            string textureName = "Items/Ores/coal/coal";

            switch (type)
            {
                case OreEnum.Silver:
                    textureName = "Items/Ores/silver/silver_" + textureNumber;
                    break;
                case OreEnum.Gold:
                    textureName = "Items/Ores/gold/gold_" + textureNumber;
                    break;
            }

            return new Ore(TextureMgr.Instance.GetTexture(textureName), position, size / 2, 3, scene, type);
        }

        public static Food FoodFactory(Random rng, Vector2 position, Vector2 size, Scene.Scene scene)
        {
            FoodEnum type = RandFoodType(rng);
            string textureName = "Items/Food/steak";

            if (type == FoodEnum.Meat)
                textureName = "Items/Food/meat";

            return new Food(TextureMgr.Instance.GetTexture(textureName), position, size / 2, 3, scene, type);
        }

        public static Chest ChestFactory(Vector2 position, Vector2 size, Scene.Scene scene)
        {
            return new Chest(TextureMgr.Instance.GetTexture("Items/Chests/chest_1"), position, size, 3, scene);
        }

        public static Key KeyFactory(Vector2 position, Vector2 size, Scene.Scene scene)
        {
            return new Key(TextureMgr.Instance.GetTexture("Items/Keys/key_1"), position, size, 3, scene);
        }

        public static Heart HeartFactory(Vector2 position, Vector2 size, Scene.Scene scene)
        {
            return new Heart(TextureMgr.Instance.GetTexture("Items/Heart/heart_1"), position, size, 3, scene);
        }
    }
}
