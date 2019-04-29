using System.Collections.Generic;

namespace Silesian_Undergrounds.Engine.Collisions
{
    public sealed class CollisionSystem
    {
        public static readonly HashSet<ICollider> Colliders = new HashSet<ICollider>();

        public static void CleanUp()
        {
            Colliders.Clear();
        }

        public static void AddColliderToSystem(ICollider collider)
        {
            Colliders.Add(collider);
        }

        public static void RemoveColliderFromSystem(ICollider collider)
        {
            Colliders.Remove(collider);
        }
    }
}
