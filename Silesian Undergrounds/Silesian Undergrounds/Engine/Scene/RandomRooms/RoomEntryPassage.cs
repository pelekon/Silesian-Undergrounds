using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Silesian_Undergrounds.Engine.Scene.RandomRooms
{
    internal enum PassageSide
    {
        PASSAGE_SIDE_UP_OR_DOWN,
        PASSAGE_SIDE_LEFT_OR_RIGHT,
    }

    class RoomEntryPassage
    {
        internal List<Point> points { get; private set; }
        internal PassageSide side { get; private set; }

        internal RoomEntryPassage(List<Point> passage, PassageSide side)
        {
            points = passage;
            this.side = side;
        }
    }
}
