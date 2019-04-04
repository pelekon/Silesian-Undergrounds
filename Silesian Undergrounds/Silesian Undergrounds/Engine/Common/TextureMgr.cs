using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Enum;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Item;

namespace Silesian_Undergrounds.Engine.Common
{
    public sealed class TextureMgr
    {
        private static TextureMgr instance = null;

        private TextureMgr() { textures = new Dictionary<string, Texture2D>(); }

        public static TextureMgr Instance
        {
            get
            {
                if (instance == null)
                    instance = new TextureMgr();

                return instance;
            }
        }

        private ContentManager contentMgr;
        private Dictionary<string, Texture2D> textures;

        public void SetCurrentContentMgr(ContentManager mgr) { contentMgr = mgr; }

        public Texture2D GetTexture(string name)
        {
            if (!textures.ContainsKey(name))
                return null;

            Texture2D returned;
            if (!textures.TryGetValue(name, out returned))
                return null;

            var method = returned.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            return (Texture2D)method.Invoke(returned, null);
        }

        public void LoadIfNeeded(string name)
        {
            if (textures.ContainsKey(name))
                return;

            Texture2D texture = contentMgr.Load<Texture2D>(name);
            textures.Add(name, texture);
        }

        public void LoadIfNeeded(Dictionary<int, string>.ValueCollection list)
        {
            foreach (var txt in list)
                LoadIfNeeded(txt);
        }

        private OreEnum RandOreType(Random random)
        {
            int randed = random.Next(1, 100);
            if (randed > 40 && randed <= 70)
                return OreEnum.Coal;
            else if (randed > 70 && randed <= 90)
                return OreEnum.Silver;
            else
                return OreEnum.Gold;

        }

        private PickableEnum RandItem(Random random)
        {
            int randed = random.Next(1, 100);
            if (randed <= 45)
                return PickableEnum.None;
            else if (randed > 45 && randed <= 75)
                return PickableEnum.Ore;
            else if (randed > 75 && randed <= 85)
                return PickableEnum.Chest;
            else
                return PickableEnum.Key;
        }

        public void GenerateItems(Scene.Scene scene, List<GameObject> pickableItems)
        {
            Random random = new Random();

            foreach (Tile pickableObject in pickableItems)
            {
                PickableEnum itemType = RandItem(random);

                if (itemType == PickableEnum.None)
                    continue;

                if (itemType == PickableEnum.Ore)
                    GenerateOre(scene, random, pickableObject);
                else if (itemType == PickableEnum.Chest)
                    GenerateChest(scene, pickableObject);
                else
                    GenerateKey(scene, pickableObject);

            }
        }

        private void GenerateOre(Scene.Scene scene, Random random, Tile pickableObject)
        {
            OreEnum type = RandOreType(random);

            int textureNumber = random.Next(1, 3);

            if (type == OreEnum.Coal)
                scene.AddObject(new Ore(LoadTexture2DByName("Items/Ores/coal/coal"), pickableObject.position, pickableObject.size / 2, 3, scene, type));
            else if (type == OreEnum.Silver)
                scene.AddObject(new Ore(LoadTexture2DByName("Items/Ores/silver/silver_" + textureNumber), pickableObject.position, pickableObject.size / 2, 3, scene, type));
            else if (type == OreEnum.Gold)
                scene.AddObject(new Ore(LoadTexture2DByName("Items/Ores/gold/gold_" + textureNumber), pickableObject.position, pickableObject.size / 2, 3, scene, type));

        }

        private void GenerateChest(Scene.Scene scene, Tile pickableObject)
        {
            scene.AddObject(new Chest(LoadTexture2DByName("Items/Chests/chest_1"), pickableObject.position, pickableObject.size, 3, scene));
        }

        private void GenerateKey(Scene.Scene scene, Tile pickableObject)
        {
            scene.AddObject(new Key(LoadTexture2DByName("Items/Keys/key_1"), pickableObject.position, pickableObject.size, 3, scene));
        }

        public Texture2D LoadTexture2DByName(string name)
        {
            return this.contentMgr.Load<Texture2D>(name);
        }


    }
}
