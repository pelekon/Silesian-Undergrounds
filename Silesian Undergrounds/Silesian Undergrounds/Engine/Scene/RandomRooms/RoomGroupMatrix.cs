using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Silesian_Undergrounds.Engine.Scene.RandomRooms
{
    internal enum RoomTileType
    {
        ROOM_TILE_NONE,
        ROOM_TILE_WALL_UP,
        ROOM_TILE_WALL_BOTTOM,
        ROOM_TILE_WALL_LEFT,
        ROOM_TILE_WALL_RIGHT,
        ROOM_TILE_GROUND,
        ROOM_TILE_PASSAGE,
    }

    internal struct RoomGroupMatrix
    {
        public Point offset;
        public RoomTileType[][] data;
    }
}
