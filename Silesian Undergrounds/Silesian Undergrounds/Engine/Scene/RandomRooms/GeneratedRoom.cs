using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        internal GeneratedRoom(RoomGroupMatrix matrix)
        {
            this.matrix = matrix;
        }

        public List<GameObject> BuildGameObjectsList()
        {
            int sizeX = matrix.data.Length;
            int sizeY = matrix.data[0].Length;

            List<GameObject> objs = new List<GameObject>(sizeX * sizeY);

            Texture2D ground = TextureMgr.Instance.GetTexture("Items/Grounds/ground_1");
            Texture2D wallUp = TextureMgr.Instance.GetTexture("top");
            Texture2D wallBottom = TextureMgr.Instance.GetTexture("bottom");
            Texture2D wallLeft = TextureMgr.Instance.GetTexture("left");
            Texture2D wallRight = TextureMgr.Instance.GetTexture("right");
            Texture2D wallUpperLeft = TextureMgr.Instance.GetTexture("top-left");
            Texture2D wallUpperRight = TextureMgr.Instance.GetTexture("top-right");
            Texture2D wallBottomLeft = TextureMgr.Instance.GetTexture("bottom-left");
            Texture2D wallBottomRight = TextureMgr.Instance.GetTexture("bottom-right");

            int wallLayer = (int)LayerEnum.Walls;

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

            return objs;
        }
    }
}
