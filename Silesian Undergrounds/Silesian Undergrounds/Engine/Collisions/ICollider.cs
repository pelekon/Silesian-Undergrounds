using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Components;

namespace Silesian_Undergrounds.Engine.Collisions
{
    public interface ICollider : IComponent
    {
        float OffsetX { get; }
        float OffsetY { get; }

        bool IsCollidingWith(BoxCollider collider);
        bool IsCollidingWith(BoxCollider collider, ref RectCollisionSides sides);
        bool IsCollidingWith(CircleCollider collider);

        void Move(Vector2 moveForce);
    }
}
