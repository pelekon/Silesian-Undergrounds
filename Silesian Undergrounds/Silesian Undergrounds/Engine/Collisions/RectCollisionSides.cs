using System;

namespace Silesian_Undergrounds.Engine.Collisions
{
    [Flags]
    public enum RectCollisionSides
    {
        SIDE_LEFT = 1,
        SIDE_RIGHT = 2,
        SIDE_BOTTOM = 4,
        SIDE_UP = 8,
    }
}
