using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

using Silesian_Undergrounds.Engine.Common;
namespace Silesian_Undergrounds.Engine.Scene {
    class TileMapRenderer {
        private List<Tile> tiles = new List<Tile>();
        private List<Gameobject> pickableItems = new List<Gameobject>();
        private int width, height;

        public List<Tile> Tiles
        {
            get
            {
                return tiles;
            }
        }

        public List<Gameobject> Pickable
        {
            get
            {
                return pickableItems;
            }
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

            foreach(var item in map)
            {
                Texture2D[][] array = item.Value;

                for (int x = 0; x < array.GetLength(0); x++)
                {
                    for (int y = 0; y < array[x].GetLength(0); y++)
                    {

                        if (array[y][x] == null)
                            continue;

                        if (item.Key == 3)
                            pickableItems.Add(new Tile(null, new Vector2(x * size, y * size), new Vector2(size, size), item.Key));
                        else
                            tiles.Add(new Tile(array[y][x], new Vector2(x * size, y * size), new Vector2(size, size), item.Key));

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
