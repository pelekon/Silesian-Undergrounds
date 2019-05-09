using System;
using System.Collections.Generic;

using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Engine.Item;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.SpecialItems;
using Silesian_Undergrounds.Engine.CommonF;
using Silesian_Undergrounds.Engine.Traps;

namespace Silesian_Undergrounds.Engine.Utils
{
    public sealed class GameObjectFactory
    {

        #region GENERATION_PARAMETERS
        private static double trapPropability = 0.1;
        private static int NUMBER_OF_SHOP_ITEMS_TYPES = 3;
        #endregion

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

        private static SpecialItemEnum RandSpecialItem(Random random)
        {
            int randed = random.Next(1, 8);
            System.Diagnostics.Debug.WriteLine(randed);
            switch (randed){
                case 1: 
                    return SpecialItemEnum.LiveBooster;
                case 2:
                    return SpecialItemEnum.HungerBooster;
                case 3:
                    return SpecialItemEnum.MovementBooster;
                case 4:
                    return SpecialItemEnum.AttackBooster;
                case 5:
                    return SpecialItemEnum.HungerImmunite;
                case 6:
                    return SpecialItemEnum.PickupDouble;
                default:
                    return SpecialItemEnum.ChestsDropBooster;
            }
        }

        private static PickableEnum RandItem(Random random, PlayerStatistic playerStatistic = null)
        {
            int maxRandValue = 100;

            if (playerStatistic != null)
                if (playerStatistic.ChestDropBooster)
                    maxRandValue += 20;

            int randed = random.Next(1, maxRandValue);

            if (randed <= 10)
                return PickableEnum.None;
            else if (randed > 10 && randed <= 25)
                return PickableEnum.Hearth;
            else if (randed > 25 && randed <= 45)
                return PickableEnum.Food;
            else if (randed > 45 && randed <= 75)
                return PickableEnum.Ore;
            else if (randed > 75 && randed <= 85)
                return PickableEnum.Key;
            else
                return PickableEnum.Chest;
        }

        #endregion

        // renders traps int the random way
        public static List<PickableItem> SceneTrapsFactory(List<GameObject> positionSources, Scene.Scene scene)
        {
            Random random = new Random();
            List<PickableItem> list = new List<PickableItem>();
            bool trapPossibility = random.NextDouble() <= trapPropability;

            foreach (var source in positionSources)
            {
                if (trapPossibility)
                    list.Add(SpikeFactory(source.position, source.size, scene));

                trapPossibility = random.NextDouble() <= trapPropability;
            }

            return list;
        }


        public static List<SpecialItem> SceneSpecialItemsFactory(List<GameObject> specialItemsPositions, Scene.Scene scene)
        {
            List<SpecialItem> specialItems = new List<SpecialItem>();
            Random random = new Random();

            foreach(var obj in specialItemsPositions)
            {
                SpecialItemEnum itemType = RandSpecialItem(random);

                specialItems.Add(SpecialItemFactory(itemType, obj.position, obj.size, scene));
            }

            return specialItems;
        }


        // renders shop pickable elements in the positions given in the positionSources arguemnt
        public static List<PickableItem> SceneShopPickableItemsFactory(List<GameObject> positionSources, Scene.Scene scene)
        {
            List<PickableItem> list = new List<PickableItem>();
            Random random = new Random();
            if (positionSources.Count != NUMBER_OF_SHOP_ITEMS_TYPES)
                return list;

           for(int i = 0; i < NUMBER_OF_SHOP_ITEMS_TYPES; i++)
            {
                switch(i)
                {
                    case 0:
                        list.Add(FoodFactory(random, positionSources[i].position, positionSources[i].size, scene, layer: (int)LayerEnum.ShopPickables, isBuyable: true));
                        break;
                    case 1:
                        list.Add(KeyFactory(positionSources[i].position, positionSources[i].size, scene, layer: (int)LayerEnum.ShopPickables, isBuyable: true));
                        break;
                    case 2:
                        list.Add(HeartFactory(positionSources[i].position, positionSources[i].size, scene, layer: (int)LayerEnum.ShopPickables, isBuyable: true));
                        break;
                }
            }
           
        
            return list;
        }


        // renders random items (hearts, chests and ores) on the map
        public static List<PickableItem> ScenePickableItemsFactory(List<GameObject> positionSources, Scene.Scene scene, PlayerStatistic playerStatistic = null)
        {
            List<PickableItem> list = new List<PickableItem>();
            Random random = new Random();

            foreach (var source in positionSources)
            {

                PickableEnum itemType = RandItem(random, playerStatistic);
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
                        Console.WriteLine("Not registered PickableItem Type in ScenePickableItemsFactory.");
                        #endif
                        break;
                }
            }

            return list;
        }

        public static SpecialItem SpecialItemFactory(SpecialItemEnum itemType, Vector2 position, Vector2 size, Scene.Scene scene)
        {
            switch (itemType)
            {
                case SpecialItemEnum.LiveBooster:
                    return new LiveBooster(TextureMgr.Instance.GetTexture("Items/Special/liveBooster"), position, size, (int)LayerEnum.SpecialItems, scene);
                case SpecialItemEnum.HungerBooster:
                    return new HungerBooster(TextureMgr.Instance.GetTexture("Items/Special/hungerBooster"), position, size, (int)LayerEnum.SpecialItems, scene);
                case SpecialItemEnum.MovementBooster:
                    return new MovementBooster(TextureMgr.Instance.GetTexture("Items/Special/movementBooster"), position, size, (int)LayerEnum.SpecialItems, scene);
                case SpecialItemEnum.AttackBooster:
                    return new AttackBooster(TextureMgr.Instance.GetTexture("Items/Special/attackBooster"), position, size, (int)LayerEnum.SpecialItems, scene);
                case SpecialItemEnum.HungerImmunite:
                    return new HungerImmunite(TextureMgr.Instance.GetTexture("Items/Special/hungerImmunite"), position, size, (int)LayerEnum.SpecialItems, scene);
                case SpecialItemEnum.PickupDouble:
                    return new PickupDouble(TextureMgr.Instance.GetTexture("Items/Special/pickupDouble"), position, size, (int)LayerEnum.SpecialItems, scene);
                default:
                    return new ChestsDropBooster(TextureMgr.Instance.GetTexture("Items/Special/chestDrop"), position, size, (int)LayerEnum.SpecialItems, scene);
            }
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


        public static Food FoodFactory(Random rng, Vector2 position, Vector2 size, Scene.Scene scene, int layer = 3, bool isBuyable = false)
        {
            FoodEnum type = RandFoodType(rng);
            string textureName = "Items/Food/steak";

            if (type == FoodEnum.Meat)
                textureName = "Items/Food/meat";

            return new Food(TextureMgr.Instance.GetTexture(textureName), position, size / 2, layer, scene, type, isBuyable: isBuyable);
        }

        public static Chest ChestFactory(Vector2 position, Vector2 size, Scene.Scene scene, bool isBuyable = false)
        {
            return new Chest(TextureMgr.Instance.GetTexture("Items/Chests/chest_1"), position, size, 3, scene, isBuyable: isBuyable);
        }

        public static Key KeyFactory(Vector2 position, Vector2 size, Scene.Scene scene, int layer = 3, bool isBuyable = false)
        {
            return new Key(TextureMgr.Instance.GetTexture("Items/Keys/key_1"), position, size, layer, scene, isBuyable: isBuyable);
        }

        public static Heart HeartFactory(Vector2 position, Vector2 size, Scene.Scene scene, bool isBuyable = false, int layer = 3)
        {
            return new Heart(TextureMgr.Instance.GetTexture("Items/Heart/heart_1"), position, size, layer, scene, isBuyable: isBuyable);
        }

        public static Spike SpikeFactory(Vector2 position, Vector2 size, Scene.Scene scene)
        {
            return new Spike(TextureMgr.Instance.GetTexture("Items/Traps/temporary_spike_1"), position, size, 4, scene);
        }
    }
}
