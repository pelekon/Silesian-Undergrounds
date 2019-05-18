using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Collisions
{
    public struct CollisionNotifyData
    {
        public CollisionNotifyData(GameObject o, ICollider c)
        {
            obj = o;
            source = c;
        }

        public GameObject obj { get; }
        public ICollider source { get; }
    }
}
