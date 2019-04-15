using System.Collections.Generic;

namespace Silesian_Undergrounds.Engine.Collisions
{
    public sealed class CollisionSystem
    {
        public static readonly HashSet<BoxCollider> Colliders = new HashSet<BoxCollider>();

        public static void CleanUp()
        {
            Colliders.Clear();
        }

        public static void AddColliderToSystem(BoxCollider collider)
        {
            Colliders.Add(collider);
        }

        public static void RemoveColliderFromSystem(BoxCollider collider)
        {
            Colliders.Remove(collider);
        }
    }
}
