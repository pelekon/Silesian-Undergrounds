using System;
using System.IO;
using System.Json;
using System.Xml;
using System.Collections.Generic;

using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Engine.Item;
using Microsoft.Xna.Framework.Graphics;

namespace Silesian_Undergrounds.Engine.Scene
{
    public class SceneManager
    {
        private static Scene currentScene;
        public static Scene GetCurrentScene() { return currentScene; }

        private static TileMapRenderer renderer = new TileMapRenderer();

        public static Scene LoadScene(String sceneName, int tileSize)
        {
            string path = "Data\\" + sceneName + ".json";
            if (!File.Exists(path))
                return null;

            Scene scene = new Scene();

            if (!LoadSceneFile(path, scene, tileSize))
                return null;

            currentScene = scene;

            return scene;
        }

        private static bool LoadSceneFile(String sceneName, Scene scene, int tileSize)
        {
            var file = File.OpenRead(sceneName);
            JsonValue json = JsonValue.Load(file);

            if (!json.ContainsKey("height") || !json.ContainsKey("width") || !json.ContainsKey("infinite") || !json.ContainsKey("layers")
                || !json.ContainsKey("tileheight") || !json.ContainsKey("tilesets"))
            {
                file.Close();
                return false;
            }

            JsonValue tileSet = json["tilesets"][0];
            string tileSetFile = "Data/" + tileSet["source"];

            if (!File.Exists(tileSetFile))
            {
                file.Close();
                return false;
            }

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
                    textures.Add(data.id, data.resource);
                    data = new TileSetData();
                    isDataNodeComplete = false;
                }

                switch (tiles.Name)
                {
                    case "tile":
                        string strId = tiles.GetAttribute("id");
                        int id = Convert.ToInt32(strId);
                        data.id = id + 1;
                        break;
                    case "image":
                        string source = tiles.GetAttribute("source");
                        source = source.Replace(".png", "");
                        data.resource = source;
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
            JsonArray layers = json["layers"] as JsonArray;

            foreach (var layer in layers)
            {
                int id = layer["id"];
                int height = layer["height"];
                int width = layer["width"];

                JsonArray tilesData = layer["data"] as JsonArray;

                Texture2D[][] tab = BuildTableWithTiles(tilesData, width, height, textures);

                tileMap.Add(id, tab);
            }

            renderer.GenerateTileMap(tileMap,tileSize);

            foreach (Tile tile in renderer.Tiles)
                scene.AddObject(tile);

            Random random = new Random();

            foreach (Tile pickableObject in renderer.Pickable)
            {
                OreEnum type = TextureMgr.Instance.RandType(random);

                if (type == OreEnum.None)
                    continue;

                if (type == OreEnum.Coal)
                    scene.AddObject(new Ore(TextureMgr.Instance.LoadTexture2DByName("coal"), pickableObject.position, pickableObject.size / 2, 3, scene, type));
                else if (type == OreEnum.Silver)
                    scene.AddObject(new Ore(TextureMgr.Instance.LoadTexture2DByName("silver"), pickableObject.position, pickableObject.size / 2, 3, scene, type));
                else if (type == OreEnum.Gold)
                    scene.AddObject(new Ore(TextureMgr.Instance.LoadTexture2DByName("gold"), pickableObject.position, pickableObject.size / 2, 3, scene, type));
            }


            file.Close();
            tileFile.Close();
            return true;
        }

        private static Texture2D[][] BuildTableWithTiles(JsonArray tiles, int width, int height, Dictionary<int, string> textures)
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
                    string name = "";
                    textures.TryGetValue(resId, out name);
                    table[w][h] = TextureMgr.Instance.GetTexture(name);
                }
            }
            
            return table;
        }
    }

    class TileSetData
    {
        public int id;
        public string resource;
    }
}
