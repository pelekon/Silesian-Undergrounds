﻿using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Components;

namespace Silesian_Undergrounds.Engine.Collisions
{
    public interface ICollider : IComponent
    {
        float OffsetX { get; }
        float OffsetY { get; }

        bool IsCollidingWith(BoxCollider collider, ref RectCollisionSides sides, Vector2 moveForce);
        bool IsCollidingWith(CircleCollider collider);

        void Move(Vector2 moveForce);

        bool triggerOnly { get; }
        bool canIgnoreTraps { get; }
        bool isAggroArea { get; }
        bool ignoreAggroArea { get; }

        void MarkAsAggroArea();
    }
}
