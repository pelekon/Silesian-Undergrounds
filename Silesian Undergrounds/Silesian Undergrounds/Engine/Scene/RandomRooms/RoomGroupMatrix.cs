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
        ROOM_TILE_CORNER_UL, // UPPER_LEFT
        ROOM_TILE_CORNER_UR, // UPPER_RIGHT,
        ROOM_TILE_CORNER_BL, // BOTTOM_LEFT
        ROOM_TILE_CORNER_BR, // BOTTOM_RIGHT
        ROOM_TILE_GROUND,
        ROOM_TILE_PASSAGE,
    }

    internal struct RoomGroupMatrix
    {
        public Point offset;
        public RoomTileType[][] data;
        public RoomSplitSide splitSide;
    }
}
