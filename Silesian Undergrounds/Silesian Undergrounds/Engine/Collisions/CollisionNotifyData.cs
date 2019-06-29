using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Collisions
{
    public struct CollisionNotifyData
    {
        public CollisionNotifyData(GameObject o, ICollider c, RectCollisionSides sides)
        {
            obj = o;
            source = c;
            collisionSides = sides;
        }

        public GameObject obj { get; }
        public ICollider source { get; }
        public RectCollisionSides collisionSides { get; }
    }
}
