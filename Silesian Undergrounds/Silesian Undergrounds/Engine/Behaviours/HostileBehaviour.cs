using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Components;
using Silesian_Undergrounds.Engine.Collisions;
using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Engine.Behaviours
{
    public class HostileBehaviour : IComponent
    {
        // Component inherited
        public Vector2 Position { get; set; }
        public Rectangle Rect { get; set; }

        public GameObject Parent { get; private set; }

        // HostileBehaviour specific
        private bool IsInCombat;
        private GameObject enemy;
        private CircleCollider aggroArea;
        private BoxCollider collider;
        private bool IsMoveNeeded;
        private TimedEventsScheduler events;
        private bool IsRangedType;
        private float MinDistToEnemy;
        AttackPattern attackPattern;

        public HostileBehaviour(GameObject parent, AttackPattern pattern, bool ranged = false, float minDist = 1)
        {
            Parent = parent;
            Position = new Vector2(0, 0);
            Rect = new Rectangle(0, 0, 0, 0);

            IsInCombat = false;
            enemy = null;
            IsMoveNeeded = false;
            IsRangedType = ranged;
            MinDistToEnemy = minDist;

            aggroArea = new CircleCollider(Parent, 70, 0, 0);
            collider = new BoxCollider(Parent, 70, 70, 0, 0, false);
            Parent.AddComponent(collider);
            Parent.AddComponent(aggroArea);
            Parent.OnCollision += NotifyCollision;

            events = new TimedEventsScheduler();
            attackPattern = pattern;
        }

        public void CleanUp()
        {
            Parent = null;
            aggroArea = null;
            collider = null;
        }

        public void Draw(SpriteBatch batch) { }

        public void RegisterSelf() { }

        public void UnRegisterSelf() { }

        public void Update(GameTime gameTime)
        {
            if (IsInCombat)
            {
                CheckDistanceToEnemy();

                if (IsMoveNeeded)
                    UpdateMovement();
                else
                    events.Update(gameTime);
            }
        }

        private void DropCombat()
        {
            IsInCombat = false;
            IsMoveNeeded = false;
            enemy = null;
            events.ClearAll();
        }

        public void NotifyCollision(object sender, CollisionNotifyData data)
        {
            if (!IsInCombat && data.source == aggroArea)
            {
                if (data.obj is Player)
                {
                    IsInCombat = true;
                    enemy = data.obj;
                    CheckDistanceToEnemy();
                    PrepareAttackEvents();
                }
            }
        }

        private void UpdateMovement()
        {
            Vector2 moveForce = new Vector2(0, 0);

            if (enemy.position.X < Parent.position.X)
                moveForce.X = -1;
            else
                moveForce.X = 1;

            if (enemy.position.Y < Parent.position.Y)
                moveForce.Y = -1;
            else
                moveForce.Y = 1;

            collider.Move(moveForce);
        }

        private void CheckDistanceToEnemy()
        {
            float dist = CircleCollider.GetDistanceBetweenPoints(Parent.position.X, Parent.position.Y, enemy.position.X, enemy.position.Y);

            if (dist >= MinDistToEnemy)
                IsMoveNeeded = true;
            else
                IsMoveNeeded = false;
        }

        private void PrepareAttackEvents()
        {
            // @TODO: Add code to init events with data from AttackPattern
            // require: implementation of AttackPatterns
        }
    }
}
