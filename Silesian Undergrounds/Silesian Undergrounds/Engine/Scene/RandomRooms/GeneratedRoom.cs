using System;
using System.Collections.Generic;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Engine.Enum;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Silesian_Undergrounds.Engine.Scene.RandomRooms
{
    public class GeneratedRoom
    {
        RoomGroupMatrix matrix;
        RoomEntryPassage passage;

        internal GeneratedRoom(RoomGroupMatrix matrix, RoomEntryPassage passage)
        {
            this.matrix = matrix;
            this.passage = passage;
        }

        public List<GameObject> BuildGameObjectsList(Scene scene)
        {
            int sizeX = matrix.data.Length;
            int sizeY = matrix.data[0].Length;
            int wallLayer = (int)LayerEnum.Walls;

            List<GameObject> objs = new List<GameObject>(sizeX * sizeY);

            var passages = BuildPassage(wallLayer);
            objs.AddRange(passages);

            if (matrix.isInvalid)
                return objs;

            Texture2D ground = TextureMgr.Instance.GetTexture("Items/Grounds/ground_1");
            Texture2D wallUp = TextureMgr.Instance.GetTexture("top");
            Texture2D wallBottom = TextureMgr.Instance.GetTexture("bottom");
            Texture2D wallLeft = TextureMgr.Instance.GetTexture("left");
            Texture2D wallRight = TextureMgr.Instance.GetTexture("right");
            Texture2D wallUpperLeft = TextureMgr.Instance.GetTexture("top-left");
            Texture2D wallUpperRight = TextureMgr.Instance.GetTexture("top-right");
            Texture2D wallBottomLeft = TextureMgr.Instance.GetTexture("bottom-left");
            Texture2D wallBottomRight = TextureMgr.Instance.GetTexture("bottom-right");

            for (int x = 0; x < sizeX; ++x)
            {
                for(int y = 0; y < sizeY; ++y)
                {
                    int posX = x + matrix.offset.X;
                    int posY = y + matrix.offset.Y;

                    switch(matrix.data[x][y])
                    {
                        case RoomTileType.ROOM_TILE_GROUND:
                            objs.Add(new Tile(ground, new Vector2(posX * ResolutionMgr.TileSize, posY * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), 0));
                            break;
                        case RoomTileType.ROOM_TILE_WALL_BOTTOM:
                            objs.Add(new Tile(wallBottom, new Vector2(posX * ResolutionMgr.TileSize, posY * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), wallLayer));
                            break;
                        case RoomTileType.ROOM_TILE_WALL_UP:
                            objs.Add(new Tile(wallUp, new Vector2(posX * ResolutionMgr.TileSize, posY * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), wallLayer));
                            break;
                        case RoomTileType.ROOM_TILE_WALL_LEFT:
                            objs.Add(new Tile(wallLeft, new Vector2(posX * ResolutionMgr.TileSize, posY * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), wallLayer));
                            break;
                        case RoomTileType.ROOM_TILE_WALL_RIGHT:
                            objs.Add(new Tile(wallRight, new Vector2(posX * ResolutionMgr.TileSize, posY * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), wallLayer));
                            break;
                        case RoomTileType.ROOM_TILE_CORNER_UL:
                            objs.Add(new Tile(wallUpperLeft, new Vector2(posX * ResolutionMgr.TileSize, posY * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), wallLayer));
                            break;
                        case RoomTileType.ROOM_TILE_CORNER_UR:
                            objs.Add(new Tile(wallUpperRight, new Vector2(posX * ResolutionMgr.TileSize, posY * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), wallLayer));
                            break;
                        case RoomTileType.ROOM_TILE_CORNER_BL:
                            objs.Add(new Tile(wallBottomLeft, new Vector2(posX * ResolutionMgr.TileSize, posY * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), wallLayer));
                            break;
                        case RoomTileType.ROOM_TILE_CORNER_BR:
                            objs.Add(new Tile(wallBottomRight, new Vector2(posX * ResolutionMgr.TileSize, posY * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), wallLayer));
                            break;
                    }
                }
            }

            var content = BuildRoomContentList(sizeX, sizeY, scene);
            objs.AddRange(content);

            return objs;
        }

        private List<GameObject> BuildPassage(int wallLayer)
        {
            List<GameObject> passageTile = new List<GameObject>();

            if (passage == null)
                return passageTile;

            if(!matrix.isInvalid)
            {
                Texture2D ground = TextureMgr.Instance.GetTexture("Items/Grounds/ground_1");
                foreach (var tile in passage.points)
                {
                    passageTile.Add(new Tile(ground, new Vector2(tile.X * ResolutionMgr.TileSize, tile.Y * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), 0));

                    if (passage.side == PassageSide.PASSAGE_SIDE_UP_OR_DOWN)
                    {
                        int posX = tile.X - matrix.offset.X;
                        int posY = (tile.Y < matrix.offset.Y) ? 0 : matrix.data[0].Length - 1;
                        matrix.data[posX][posY] = RoomTileType.ROOM_TILE_GROUND;
                    }
                    else
                    {
                        int posX = (tile.X < matrix.offset.X) ? 0 : matrix.data.Length - 1;
                        int posY = tile.Y - matrix.offset.Y;
                        matrix.data[posX][posY] = RoomTileType.ROOM_TILE_GROUND;
                    }
                }
            }
            else
            {
                if (passage.side == PassageSide.PASSAGE_SIDE_UP_OR_DOWN)
                {
                    Texture2D wallUp = TextureMgr.Instance.GetTexture("top");
                    Texture2D wallBottom = TextureMgr.Instance.GetTexture("bottom");

                    foreach (var tile in passage.points)
                    {
                        if (tile.Y == (matrix.offset.Y - 1))
                            passageTile.Add(new Tile(wallBottom, new Vector2(tile.X * ResolutionMgr.TileSize, tile.Y * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), wallLayer));
                        else
                            passageTile.Add(new Tile(wallUp, new Vector2(tile.X * ResolutionMgr.TileSize, tile.Y * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), wallLayer));
                    }
                }
                else
                {
                    Texture2D wallLeft = TextureMgr.Instance.GetTexture("left");
                    Texture2D wallRight = TextureMgr.Instance.GetTexture("right");

                    foreach (var tile in passage.points)
                    {
                        if (tile.X == (matrix.offset.X - 1))
                            passageTile.Add(new Tile(wallRight, new Vector2(tile.X * ResolutionMgr.TileSize, tile.Y * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), wallLayer));
                        else
                            passageTile.Add(new Tile(wallLeft, new Vector2(tile.X * ResolutionMgr.TileSize, tile.Y * ResolutionMgr.TileSize), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), wallLayer));
                    }
                }
            }

            return passageTile;
        }

        private List<GameObject> BuildRoomContentList(int sizeX, int sizeY, Scene scene)
        {
            List<GameObject> content = new List<GameObject>();
            Random rng = TrueRng.GetInstance().GetRandom();

            int enemyChance = rng.Next(0, 100);
            bool isEnemySpawned = enemyChance <= 40;
            Vector2 center = GetRoomCenterPosition(sizeX, sizeY);

            bool isCenterSpawned = false;

            if (isEnemySpawned)
            {
                isCenterSpawned = true;
                content.Add(EnemyFactory.GetRandomEnemy(center));
            } 
            else
            {
                int chestChance = rng.Next(0, 100);

                if (chestChance <= 40)
                {
                    content.Add(GameObjectFactory.ChestFactory(center, new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), scene));
                    isCenterSpawned = true;
                }
            }

            int amountOfItems = rng.Next(0, 60);
            double temp = amountOfItems / 10;
            temp = Math.Floor(temp);
            amountOfItems = (int)temp;

            if (amountOfItems > 3)
                amountOfItems = 3;
            else
            {
                if (amountOfItems == 0 && !isCenterSpawned)
                {
                    int roll = rng.Next(0, 100);
                    if (roll <= 48)
                        amountOfItems = 1;
                }
            }

            for(int i = 0; i < amountOfItems; ++i)
            {
                int x = 0;
                int y = 0;
                switch(i)
                {
                    case 0:
                        x = (1 + matrix.offset.X) * ResolutionMgr.TileSize;
                        y = (1 + matrix.offset.Y) * ResolutionMgr.TileSize;
                        break;
                    case 1:
                        x = (2 + matrix.offset.X) * ResolutionMgr.TileSize;
                        y = (sizeY - 1 + matrix.offset.Y) * ResolutionMgr.TileSize;
                        break;
                    case 2:
                        x = (sizeX - 1 + matrix.offset.X) * ResolutionMgr.TileSize;
                        y = (sizeY - 1 + matrix.offset.Y) * ResolutionMgr.TileSize;
                        break;
                }

                var ob = GameObjectFactory.GetRandomPickableItem(new Vector2(x, y), scene);
                if (ob != null)
                    content.Add(ob);
            }

            return content;
        }

        private Vector2 GetRoomCenterPosition(int sizeX, int sizeY)
        {
            int x = sizeX / 2;
            int y = sizeY / 2;

            x += matrix.offset.X;
            y += matrix.offset.Y;

            x *= ResolutionMgr.TileSize;
            y *= ResolutionMgr.TileSize;

            return new Vector2(x, y);
        }
    }
}
