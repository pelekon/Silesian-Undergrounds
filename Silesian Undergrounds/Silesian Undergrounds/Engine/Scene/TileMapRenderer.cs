using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Silesian_Undergrounds.Engine.Scene {
    class TileMapRenderer {
        private List<Tile> tiles = new List<Tile>();
        private int width, height;

        public List<Tile> Tiles
        {
            get
            {
                return tiles;
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
                        tiles.Add(new Tile(array[y][x], new Vector3(x * size, y * size, item.Key), new Vector2(size, size))); //roboczo, żeby generować tile gdzie jest 1 i puste gdzie 0

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
