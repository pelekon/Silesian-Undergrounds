using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Enum;

namespace Silesian_Undergrounds.Engine.Scene {
    class TileMapRenderer {
        private List<Tile> tiles = new List<Tile>();
        private List<GameObject> pickableItems = new List<GameObject>();
        private List<GameObject> shopPickableItems = new List<GameObject>();
        private List<GameObject> traps = new List<GameObject>();
        private List<GameObject> grounds = new List<GameObject>();
        private List<Tile> transitions = new List<Tile>();
        private List<GameObject> enemies = new List<GameObject>();
        private int width, height;

        public List<Tile> Transitions
        {
            get
            {
                return transitions;
            }
        }

        public List<Tile> Tiles
        {
            get
            {
                return tiles;
            }
        }

        public List<GameObject> Traps
        {
            get
            {
                return traps;
            }
        }

        public List<GameObject> ShopPickables
        {
            get
            {
                return shopPickableItems;
            }
        }

        public List<GameObject> Pickable
        {
            get
            {
                return pickableItems;
            }
        }

        public List<GameObject> Grounds
        {
            get
            {
                return grounds;
            }
        }

        public List<GameObject> Enemies
        {
            get { return enemies; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public void GenerateTileMap(Dictionary<int, Texture2D[][]> map, int size)
        {
            tiles = new List<Tile>();
            pickableItems = new List<GameObject>();
            grounds = new List<GameObject>();
            transitions = new List<Tile>();
            enemies = new List<GameObject>();

            foreach (var item in map)
            {
                Texture2D[][] array = item.Value;

                for (int x = 0; x < array.GetLength(0); x++)
                {
                    for (int y = 0; y < array[x].GetLength(0); y++)
                    {

                        if (array[y][x] == null)
                            continue;

                        switch(item.Key)
                        {
                            case (int)LayerEnum.Background:
                                grounds.Add(new Tile(null, new Vector2(x * size, y * size), new Vector2(size, size), item.Key));
                                break;
                            case (int)LayerEnum.Pickables: 
                                pickableItems.Add(new Tile(null, new Vector2(x * size, y * size), new Vector2(size, size), item.Key));
                                break;
                            case (int)LayerEnum.Traps:
                                traps.Add(new Tile(null, new Vector2(x * size, y * size), new Vector2(size, size), item.Key));
                                break;
                            case (int)LayerEnum.Transitions:
                                transitions.Add(new Tile(array[y][x], new Vector2(x * size, y * size), new Vector2(size, size), item.Key));
                                break;
                            case (int)LayerEnum.Enemies:
                                enemies.Add(new Tile(null, new Vector2(x * size, y * size), new Vector2(size, size), item.Key));
                                break;
                            case (int)LayerEnum.ShopPickables:
                                shopPickableItems.Add(new Tile(null, new Vector2(x * size, y * size), new Vector2(size, size), item.Key));
                                break;
                            default:
                                tiles.Add(new Tile(array[y][x], new Vector2(x * size, y * size), new Vector2(size, size), item.Key));
                                break;
                        }

                        width = (x + 1) * size;
                        height = (y + 1) * size;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
                tile.Draw(spriteBatch);
        }
    }
}
