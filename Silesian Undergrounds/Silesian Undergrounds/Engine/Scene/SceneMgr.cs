using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Silesian_Undergrounds.Engine.Utils;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Item;
using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Engine.Scene.RandomRooms;
using System.Threading;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Config;

namespace Silesian_Undergrounds.Engine.Scene
{
    public class SceneManager
    {
        private SceneManager() {}

        #region SCENE_MANAGER_VARIABLES
        private static Scene _currentScene;
        public static Scene GetCurrentScene() { return _currentScene; }
        private static readonly TileMapRenderer Renderer = new TileMapRenderer();
        private static PlayerStatistic playerStatistic;
        #endregion

        public static Scene LoadScene(string sceneName, int tileSize)
        {
            var fileName = sceneName + Constants.JsonFileExtension;
            var path = Path.Combine(Constants.DataDirectory, fileName);
        

            if (!File.Exists(path)) return null;
            if(playerStatistic == null)
            {
                var initPlayerConfig = ConfigMgr.PlayerConfig;
                playerStatistic = new PlayerStatistic(initPlayerConfig);
            }

            var scene = new Scene(playerStatistic);

            if (!LoadSceneFile(path, scene, tileSize)) return null;

            _currentScene = scene;
            scene.player.SetPosition(Renderer.position * new Vector2(64, 64));
            return scene;
        }

        public static void ClearPlayerStatistics()
        {
            playerStatistic = null;
        }

        private static bool LoadSceneFile(string filePath, Scene scene, int tileSize)
        {
            SceneFile sceneFile;
            using (var file = File.OpenText(filePath))
            {
                try  { sceneFile = (SceneFile)new JsonSerializer().Deserialize(file, typeof(SceneFile)); }
                catch (JsonSerializationException e)
                {
                    #if DEBUG
                        Console.WriteLine(e.Message);
                    #endif
                    return false;
                }

            }
            if (sceneFile.TileSets.Count < 1) return false;
            var tileSetFile = Path.Combine(Constants.DataDirectory, sceneFile.TileSets[0].Source);

            if (!File.Exists(tileSetFile)) return false;

            // Read tileset file to get resources before build of tile map
            var tileFile = File.OpenRead(tileSetFile);
            XmlReader tiles = XmlReader.Create(tileFile);

            Dictionary<int, string> textures = new Dictionary<int, string>();
            TileSetData data = new TileSetData();
            bool isDataNodeComplete = false;

            while(tiles.Read())
            {
                if (isDataNodeComplete)
                {
                    textures.Add(data.Id, data.Resource);
                    data = new TileSetData();
                    isDataNodeComplete = false;
                }

                switch (tiles.Name)
                {
                    case "tile":
                        string strId = tiles.GetAttribute("id");
                        int id = Convert.ToInt32(strId);
                        data.Id = id + 1;
                        break;
                    case "image":
                        string source = tiles.GetAttribute("source");
                        source = source.Replace(".png", "");
                        data.Resource = source;
                        isDataNodeComplete = true;
                        break;
                }
            }

            #if DEBUG
            Console.WriteLine("All textures for current tileset:");
            foreach (var node in textures)
                Console.WriteLine("ID: " + node.Key + " Resource: " + node.Value);
            #endif

            TextureMgr.Instance.LoadIfNeeded(textures.Values);

            Dictionary<int, Texture2D[][]> tileMap = new Dictionary<int, Texture2D[][]>();

            foreach (var layer in sceneFile.Layers)
            {
                var tab = BuildTableWithTiles(layer.Data, layer.Width, layer.Height, textures);

                tileMap.Add(layer.Id, tab);
            }

            RoomGenerator roomGenerator = null;

            if (tileMap.ContainsKey((int)LayerEnum.RandomRooms))
            {
                roomGenerator = new RoomGenerator();
                Thread thread = new Thread(new ThreadStart(() => roomGenerator.GenerateRooms(tileMap[(int)LayerEnum.RandomRooms])));
                thread.Start();
            }

            Renderer.GenerateTileMap(tileMap,tileSize);

            foreach (Tile tile in Renderer.Tiles)
                scene.AddObject(tile);

            foreach (Tile tile in Renderer.Transitions)
                scene.AddTransition(tile);

            List<Ground> generatedGround = GroundTextureFactory.GroundFactory(Renderer.Grounds);
            foreach (var ground in generatedGround)
            {
                ground.SetScene(scene);
                scene.AddObject(ground);
            }

            List<PickableItem> generatedItems = GameObjectFactory.ScenePickableItemsFactory(Renderer.Pickable, scene);
            foreach(var obj in generatedItems)
            {
                obj.SetScene(scene);
                scene.AddObject(obj);
            }

            List<PickableItem> generatedTraps = GameObjectFactory.SceneTrapsFactory(Renderer.Traps, scene);
            foreach (var obj in generatedTraps)
            {
                obj.SetScene(scene);
                scene.AddObject(obj);
            }

            List<PickableItem> generatedShopPickables = GameObjectFactory.SceneShopPickableItemsFactory(Renderer.ShopPickables, scene);

            foreach (var obj in generatedShopPickables)
            {
                obj.SetScene(scene);
                scene.AddObject(obj);
            }

            List<GameObject> generatedEnemies = EnemyFactory.GenerateEnemiesForScene(Renderer.Enemies);
            foreach (var obj in generatedEnemies)
                scene.AddObject(obj);


            List<SpecialItem> specialItems = GameObjectFactory.SceneSpecialItemsFactory(Renderer.SpecialItems, scene, playerStatistic);
            foreach (var item in specialItems)
            {
                scene.AddObject(item);
            }

            if (roomGenerator != null)
            {
                while (!roomGenerator.isJobDone) { }

                foreach (var room in roomGenerator.result)
                {
                    var list = room.BuildGameObjectsList();
                    foreach (var obj in list)
                        scene.AddObject(obj);
                }
            }

            tileFile.Close();
            return true;
        }

        private static Texture2D[][] BuildTableWithTiles(IReadOnlyList<int> tiles, int width, int height, Dictionary<int, string> textures)
        {
            Texture2D[][] table = new Texture2D[width][];
            for (int i = 0; i < width; ++i)
                table[i] = new Texture2D[height];

            int counter = 0;
            for(int w = 0; w < width; ++w)
            {
                for(int h = 0; h < height; ++h)
                {
                    int resId = tiles[counter];
                    counter++;
                    if (resId == 0)
                        continue;
                    textures.TryGetValue(resId, out var name);
                    table[w][h] = TextureMgr.Instance.GetTexture(name);
                }
            }

            return table;
        }
    }

    internal class TileSetData
    {
        public int Id;
        public string Resource;
    }
}
